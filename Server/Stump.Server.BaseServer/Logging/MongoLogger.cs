using System;
using MongoDB.Bson;
using MongoDB.Driver;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.Core.Threading;
using Stump.ORM;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.BaseServer.Logging
{
    public class MongoLogger : Singleton<MongoLogger>
    {
        [Variable(Priority = 10, DefinableRunning = true)] 
        public static bool IsMongoLoggerEnabled = false;

        [Variable(Priority = 10, DefinableRunning = true)]
        public static DatabaseConfiguration MongoDBConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            DbName = "stump_logs",
            Port = "3307",
            User = "root",
            Password = ""
        };

        private IMongoDatabase m_database;

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            if (!IsMongoLoggerEnabled)
                return;

            var client = new MongoClient($"mongodb://{MongoDBConfiguration.User}:{MongoDBConfiguration.Password}@{MongoDBConfiguration.Host}:{MongoDBConfiguration.Port}/" +
                $"{MongoDBConfiguration.DbName}?authMechanism = SCRAM-SHA-1&maxPoolSize=10000");

            m_database = client.GetDatabase(MongoDBConfiguration.DbName);
        }

        public bool Insert(string collection, BsonDocument document)
        {            
            if (!IsMongoLoggerEnabled)
            {
                if (m_database == null)
                    return false;

                m_database = null;

                return false;
            }

            if (m_database == null)
                Initialize();

            if (m_database != null)
                m_database.GetCollection<BsonDocument>(collection).InsertOneAsync(document);
            else
                return false;

            return true;
        }
    }
}