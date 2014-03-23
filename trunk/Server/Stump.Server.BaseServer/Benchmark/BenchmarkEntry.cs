using System;
using System.Collections;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Benchmark
{
    public class BenchmarkEntry
    {
        public string MessageType
        {
            get;
            set;
        }

        public TimeSpan Timestamp
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public Hashtable AdditionalProperties
        {
            get;
            set;
        }

        public BenchmarkingType BenchmarkingType
        {
            get;
            set;
        }

        public static BenchmarkEntry Create(string type, TimeSpan timestamp)
        {
            return new BenchmarkEntry
            {
                BenchmarkingType = BenchmarkManager.BenchmarkingType,
                Date = DateTime.Now,
                Timestamp = timestamp,
                MessageType = type,
            };
        }

        public static BenchmarkEntry Create(string type, TimeSpan timestamp, params object[] additionalProperties)
        {
            if (additionalProperties.Length % 2 != 0)
                throw new ArgumentException("additionalProperties");

            var hashTable = new Hashtable();
            for (int i = 0; i < additionalProperties.Length; i+=2)
            {
                hashTable.Add(additionalProperties[i], additionalProperties[i + 1]);
            }

            return new BenchmarkEntry
            {
                BenchmarkingType = BenchmarkManager.BenchmarkingType,
                Date = DateTime.Now,
                Timestamp = timestamp,
                MessageType = type,
                AdditionalProperties = hashTable
            };
        }
    }
}