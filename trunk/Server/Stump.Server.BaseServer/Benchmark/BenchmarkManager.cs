using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Stump.Core.Attributes;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Benchmark
{
    public class BenchmarkManager : Singleton<BenchmarkManager>
    {
        private readonly List<BenchmarkEntry> m_entries = new List<BenchmarkEntry>();

        [Variable(true)]
        public static bool Enable = true;

        [Variable(true)]
        public static BenchmarkingType BenchmarkingType = BenchmarkingType.Complete;

        [Variable(true)]
        public static int EntriesLimit = 1000;

        public ReadOnlyCollection<BenchmarkEntry> Entries
        {
            get
            {
                return m_entries.AsReadOnly();
            }
        }

        public void RegisterEntry(BenchmarkEntry entry)
        {
            lock (m_entries)
            {
                m_entries.Add(entry);
            }

            if (m_entries.Count >= EntriesLimit)
            {
                lock (m_entries)
                {
                    m_entries.RemoveRange(0, EntriesLimit / 4);
                }
            }
        }

        public void ClearResults()
        {
            m_entries.Clear();
        }

        public BenchmarkEntry[] GetEntries(Type message)
        {
            return m_entries.Where(entry => entry.MessageType == message).ToArray();
        }

        public BenchmarkEntry[] SortEntries()
        {
            return m_entries.OrderByDescending(entry => entry.Timestamp).ToArray();
        }

        public BenchmarkEntry[] SortEntries(int limit)
        {
            return m_entries.OrderByDescending(entry => entry.Timestamp).Take(limit).ToArray();
        }

        public Dictionary<Type, Tuple<TimeSpan, int>> GetEntriesSummary()
        {
            var sortedEntries = SortEntries();
            var result = new Dictionary<Type, Tuple<TimeSpan, int>>();

            foreach (var entries in sortedEntries.GroupBy(entry => entry.MessageType))
            {
                var average = (long)entries.Average(entry => entry.Timestamp.Ticks);

                result.Add(entries.Key, Tuple.Create(new TimeSpan(average), entries.Count()));
            }

            return result;
        }

        public BenchmarkEntry GetHighestEntry(Type message)
        {
            return m_entries.
                Where(entry => entry.MessageType == message).
                OrderByDescending(entry => entry.Timestamp).
                First();
        }

        public string GenerateReport()
        {
            var builder = new StringBuilder();

            builder.AppendFormat("Benchmarking report - {0} entries\n", m_entries.Count);

            var summary = GetEntriesSummary();

            foreach (var entry in summary)
            {
                builder.AppendFormat("{0} {1}ms ({2} entries)\n", entry.Key.Name, entry.Value.Item1.TotalMilliseconds, entry.Value.Item2);
            }

            return builder.ToString();
        }
    }
}