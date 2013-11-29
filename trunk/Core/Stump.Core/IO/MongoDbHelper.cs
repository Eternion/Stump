using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Stump.Core.IO
{
    public class MongoDbHelper
    {
        public string Username;

        public string Password;

        public string Host;

        public string Port;

        private readonly string Database;

        public MongoDbHelper(string username, string password, string host, string port, string database)
        {
            Username = username;
            Password = password;
            Host = host;
            Port = port;
            Database = database;
        }

        private MongoDatabase Connect()
        {
            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(Host) &&
                string.IsNullOrEmpty(Port) && string.IsNullOrEmpty(Database))
                return null;

            var client = new MongoClient(String.Format("mongodb://{0}:{1}@{2}:{3}", Username, Password, Host, Port));
            var server = client.GetServer();
            var database = server.GetDatabase(Database);

            return database;
        }

        public bool Insert(string collection, BsonDocument document)
        {
            var database = Connect();

            if (database != null)
                database.GetCollection<BsonDocument>(collection).Insert(document);
            else
                return false;

            return true;
        }
    }
}
