using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using static System.Collections.Specialized.BitVector32;


namespace ConsoleApp5
{
    internal class AdoNet
    {
        private static AdoNet adoNetExemp = null;
        private AdoNet() 
        {
            CreateTable();

        }
        public static AdoNet newAdoNet()
        {
            if(adoNetExemp  == null) 
            {
                adoNetExemp= new AdoNet();
            }
            return adoNetExemp;
        }

        public enum Tables
        {
            id,
            chatId,
            username,
            threadId
        }

        public static void CreateTable()
        {
            using (SqliteConnection sqliteConnection = new SqliteConnection("DataSource=clientsBase.db;mode=readwritecreate"))
            {
                sqliteConnection.Open();
                var sqlCommand = sqliteConnection.CreateCommand();
                sqlCommand.CommandText = "CREATE TABLE clientsBase (id INTEGER PRIMARY KEY AUTOINCREMENT, chatId INTEGER, username TEXT, threadId INTEGER,  messageCount INTEGER, UNIQUE(chatId));";
                sqlCommand.ExecuteNonQueryAsync();
            }

        }
        public static void TableCount()
        {
            using (SqliteConnection sqliteConnection = new SqliteConnection("DataSource=clientsBase.db;mode=readwritecreate"))
            {
                sqliteConnection.Open();
                var sqlCommand = sqliteConnection.CreateCommand();
                sqlCommand.CommandText = "SELECT COUNT(*) FROM clientsBase;";
                sqlCommand.ExecuteScalarAsync();
            }

        }
        public static async Task<long> GetClientId(string topicName)
        {
            using (SqliteConnection sqliteConnection = new SqliteConnection("DataSource=clientsBase.db;mode=readwritecreate"))
            {
                long chatId = 0;
                sqliteConnection.Open();
                var sqlCommand = sqliteConnection.CreateCommand();
                sqlCommand.CommandText = $"SELECT * FROM clientsBase;";
                var tableReader = await sqlCommand.ExecuteReaderAsync();

                while (tableReader.Read())
                {
                    string username = (string)tableReader[$"username"];
                    chatId = (long)tableReader[$"chatId"];
                    if (topicName.Contains(username) && topicName.Contains(Convert.ToString(chatId))) 
                    {
                        chatId = (long)tableReader[$"chatId"];

                    }
                }

                return chatId;
            }

        }
        public static async Task<Client> TableSelect(Tables columnName, long chatId)
        {
            Client client = new Client(null,1);
            client.existance = false;
            
            string action = "";
            switch (columnName)
            {
                case Tables.id:
                    action = "id"; break;
                case Tables.chatId:
                    action = "chatId"; break;
                case Tables.username:
                    action = "username"; break;
                case Tables.threadId:
                    action = "threadId"; break;
            }
            using (SqliteConnection sqliteConnection = new SqliteConnection("DataSource=clientsBase.db;mode=readwritecreate"))
            {
                
                sqliteConnection.Open();
                var sqlCommand = sqliteConnection.CreateCommand();
                //sqlCommand.CommandText = $"SELECT {action} FROM clientsBase;";
                sqlCommand.CommandText = $"SELECT * FROM clientsBase;";
                var tableReader = await sqlCommand.ExecuteReaderAsync();

                while (tableReader.Read())
                {
                    long id = (long)tableReader[$"chatId"];
                    if (id == chatId)
                    {
                        
                        
                        client = new Client((string)tableReader["username"],id);
                        client.threadId = Convert.ToInt32(tableReader["threadId"]);


                        client.existance = true;
                        break;
                    }
                }

                return client;
            }

        }
        //добавляет клиента в базу
        public static async void TableInsert(long chatId, string username, int threadId)
        {
            
            using (SqliteConnection sqliteConnection = new SqliteConnection("DataSource=clientsBase.db;mode=readwritecreate"))
            {
                sqliteConnection.Open();
                var sqlCommand = sqliteConnection.CreateCommand();
                sqlCommand.CommandText = $"INSERT INTO clientsBase (chatId, username, threadId) VALUES ({chatId},\"{username}\",{threadId});";
                await sqlCommand.ExecuteNonQueryAsync();
                //TableInsert (chatId, username, threadId); 
            }
        }
     

    }
}
