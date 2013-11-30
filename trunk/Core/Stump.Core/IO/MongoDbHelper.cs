using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Stump.Core.IO
{
    public class MongoDbHelper
    {
        private static MongoDatabase Database;

        public MongoDbHelper(string username, string password, string host, string port, string database)
        {
            var client = new MongoClient(String.Format("mongodb://{0}:{1}@{2}:{3}", username, password, host, port));
            var server = client.GetServer();
            Database = server.GetDatabase(database);
        }

        public bool Insert(string collection, BsonDocument document)
        {
            if (Database != null)
                Database.GetCollection<BsonDocument>(collection).Insert(document);
            else
                return false;

            return true;
        }
    }
}
