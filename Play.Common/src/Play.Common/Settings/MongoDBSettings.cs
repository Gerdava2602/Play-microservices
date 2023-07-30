namespace Play.Common.Settings
{
    public class MongoDBSettings
    {
        //MongoDBSettings is a class that will be used to store the settings for the MongoDB database
        //init is used to set the value of the property only once
        public string Host { get; init; }
        public int Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}