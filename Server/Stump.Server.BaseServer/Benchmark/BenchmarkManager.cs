using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Stump.Core.Attributes;
using Stump.Core.Pool;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Benchmark
{
    public class BenchmarkManager : Singleton<BenchmarkManager>
    {
        private readonly List<BenchmarkEntry> m_entries = new List<BenchmarkEntry>();
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        [Variable(true)]
        public static bool Enable = true;

        [Variable(true)]
        public static BenchmarkingType BenchmarkingType = BenchmarkingType.Complete;

        [Variable(true)]
        public static int EntriesLimit = 10000;

        public ReadOnlyCollection<BenchmarkEntry> Entries
        {
            get
            {
                lock (m_entries)
                {
                    return m_entries.AsReadOnly();
                }
            }
        }

        public void Add(BenchmarkEntry entry)
        {
            lock (m_entries)
            {
                m_entries.Add(entry);
            }

            if (m_entries.Count < EntriesLimit)
                return;

            lock (m_entries)
            {
                m_entries.RemoveRange(0, EntriesLimit / 4);
            }
        }

        public void AddRange(IEnumerable<BenchmarkEntry> entries)
        {
            lock (m_entries)
            {
                m_entries.AddRange(entries);
            }

            if (m_entries.Count < EntriesLimit)
                return;

            lock (m_entries)
            {
                m_entries.RemoveRange(0, EntriesLimit / 4);
            }

        }

        public void ClearResults()
        {
            lock (m_entries)
            {
                m_entries.Clear();
            }
        }

        public string GenerateReport(IEnumerable<BenchmarkEntry> entries)
        {
            var sortedEntries = entries.OrderByDescending(x => x.Timestamp);

            var builder = new StringBuilder();

            builder.AppendFormat("Benchmarking report - {0} entries\n", m_entries.Count);  

            foreach (var group in sortedEntries.GroupBy(x => x.MessageType))
            {
                var average = (long)group.Average(x => x.Timestamp.TotalMilliseconds);

                builder.AppendFormat("{0} {1}ms ({2} entries)\n", group.Key, average, group.Count());
            }

            return builder.ToString();
        }

        public string GenerateReport()
        {
            lock (m_entries)
            {
                return GenerateReport(m_entries);
            }
        }

        public int PopId()
        {
            return m_idProvider.Pop();
        }
    }
}