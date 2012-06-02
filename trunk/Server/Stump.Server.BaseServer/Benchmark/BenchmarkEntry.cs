using System;
using System.Collections;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Benchmark
{
    public class BenchmarkEntry
    {
        public Type MessageType
        {
            get;
            set;
        }

        public Message Message
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

        public static BenchmarkEntry Create(TimeSpan timestamp, Message message)
        {
            return new BenchmarkEntry()
            {
                BenchmarkingType = BenchmarkManager.BenchmarkingType,
                Date = DateTime.Now,
                Timestamp = timestamp,
                MessageType = message.GetType(),
                Message = message,
            };
        }
    }
}