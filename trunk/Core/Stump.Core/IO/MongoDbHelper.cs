using MongoDB.Bson;
using MongoDB.Driver;

namespace Stump.Core.IO
{
    public static class MongoDbHelper
    {
        private static MongoDatabase Connect()
        {
            var client = new MongoClient("mongodb://server:dxdv6gy7@94.242.228.147:3307");
            var server = client.GetServer();
            var stump_logs = server.GetDatabase("stump_logs");

            return stump_logs;
        }

        public static bool Insert(string collection, BsonDocument document)
        {
            var database = Connect();

            database.GetCollection<BsonDocument>(collection).Insert(document);

            return true;
        }
    }
}
