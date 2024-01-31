namespace ConsoleApp5
{
    public class UpdateDeserialisation
    {
        public long id { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public bool? is_forum { get; set; }
    }


    public class ForumTopicCreated
    {
        public string name { get; set; }
        public int icon_color { get; set; }
    }

    public class From
    {
        public long id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
        public string language_code { get; set; }
        public bool is_premium { get; set; }
    }

    public class Message
    {
        public long message_id { get; set; }
        public From from { get; set; }
        public UpdateDeserialisation chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
        public long? message_thread_id { get; set; }
        public ReplyToMessage reply_to_message { get; set; }
        public bool? is_topic_message { get; set; }
    }

    public class ReplyToMessage
    {
        public long message_id { get; set; }
        public From from { get; set; }
        public UpdateDeserialisation chat { get; set; }
        public long date { get; set; }
        public long message_thread_id { get; set; }
        public ForumTopicCreated forum_topic_created { get; set; }
        public bool is_topic_message { get; set; }
    }

    public class Result
    {
        public int update_id { get; set; }
        public Message message { get; set; }
    }

    public class Root
    {
        public bool ok { get; set; }
        public List<Result> result { get; set; }
    }

}
