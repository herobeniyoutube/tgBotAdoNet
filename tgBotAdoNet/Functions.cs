using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal static class Functions
    {
        //ищет в json по имени клиента айди чата с ним
        public static long GetClientId(string username)
        {
            long chatId = 0;
            string json = File.ReadAllText("clients.json");
            List<Client> clientsTemp = JsonSerializer.Deserialize<List<Client>>(json);
            foreach (Client current in clientsTemp)
            {
                if (current.username == username)
                {
                    chatId = current.chatId; break;

                }
            }
            return chatId;
        }
     
        //берет из json айди треда, который закреплен за клиентом
        public static int GetThreadId(long chatId)
        {
            int threadId = 0;
            string json = File.ReadAllText("clients.json");
            List<Client> clientsTemp = JsonSerializer.Deserialize<List<Client>>(json);
            foreach (Client current in clientsTemp)
            {
                if (current.chatId == chatId)
                {
                    threadId = current.threadId; break;

                }
            }
            return threadId;
        }
        //создает топик, возвращает объект формутопик
        async static public Task<int> CreateTopicAsync(HttpClient client, long forumId, string clientName, long chatId)
        {
            var forumTopicResponse = await client.GetAsync($"createForumTopic?chat_id={forumId}&name={clientName} ID:{chatId}");


            string jsonForumTopic = await forumTopicResponse.Content.ReadAsStringAsync();
            RootTopic forumTop = JsonSerializer.Deserialize<RootTopic>(jsonForumTopic);
            int messageThreadId = forumTop.result.message_thread_id;


            return forumTop.result.message_thread_id;
        }
        //проверяет, новый ли клиент и есть ли файл с данными клиентов, запускает создание топика, если клиент новый
        //async static public Task<int> isClientNew(long chatId, string clientName, HttpClient clientHttp, long forumId)
        //{
        //    Client client = new Client(clientName, chatId);
        //    List<Client> clients = new List<Client>()
        //{
        //    client
        //};
        //    string json;
        //    bool clientExist = false;
        //    int threadId = 0;
        //    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
        //    jsonSerializerOptions.WriteIndented = true;
        //    jsonSerializerOptions.IncludeFields = true;

        //    if (File.Exists("clients.json"))
        //    {
        //        json = File.ReadAllText("clients.json");
        //        List<Client> clientsTemp = JsonSerializer.Deserialize<List<Client>>(json);
        //        foreach (Client a in clientsTemp)
        //        {
        //            if (a.chatId == chatId)
        //            {
        //                clientExist = true;
        //                break;

        //            }
        //        }
        //        if (!clientExist)
        //        {
        //            threadId = await CreateTopicAsync(clientHttp, forumId, clientName, chatId);
        //            client.threadId = threadId;
        //            clientsTemp.Add(client);
        //            json = JsonSerializer.Serialize(clientsTemp, jsonSerializerOptions);
        //            File.WriteAllText("clients.json", json);
        //            clientExist = true;

        //        }

        //    }
        //    else
        //    {

        //        threadId = await CreateTopicAsync(clientHttp, forumId, clientName, chatId);
        //        client.threadId = threadId;
        //        json = JsonSerializer.Serialize(clients);
        //        File.AppendAllText("clients.json", json);
        //        clientExist = true;
        //    }


        //    return threadId;
        //}
        async static public Task<int> isClientNewNew(long chatId, string clientName, HttpClient clientHttp, long forumId)
        {
            
            bool clientExist = false;
            int threadId = 0;   
            if (File.Exists("clientsBase.db"))
            {
                //ищет клиента в базе данных
                Client client = await AdoNet.TableSelect(AdoNet.Tables.chatId, chatId);
                clientExist = client.existance;
                threadId = client.threadId;
                
                
                if (!clientExist) 
                {
                    threadId = await CreateTopicAsync(clientHttp, forumId, clientName, chatId);
                    AdoNet.TableInsert(chatId, clientName, threadId);
                }
            }
             
            return threadId;
        }
        //хендлер апдейтов
        async static public Task<Root> UpdatesAsync(HttpClient client, long offset)
        {

            var response = await client.GetAsync($"getUpdates?offset={offset}&timeout=3");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            Root updates = JsonSerializer.Deserialize<Root>(jsonResponse);
            return updates;
        }

        async static public void TransferFromJson()
        {

            var clientsJson = File.ReadAllBytes("clients.json");
            var jsons = JsonSerializer.Deserialize<List<Client>>(clientsJson);
            for (int i = 0; i < jsons.Count - 1; i++)
            {
                Client client = jsons[i];
                AdoNet.TableInsert(client.chatId, client.username, client.threadId);
            }
        }
    }
}
