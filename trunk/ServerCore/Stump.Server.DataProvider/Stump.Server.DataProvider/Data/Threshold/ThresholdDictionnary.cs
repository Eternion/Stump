
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.DataProvider.Data.Threshold
{
    public class ThresholdDictionnary
    {
        private readonly Dictionary<uint, long> m_levels;

        private readonly string m_name;

        public ThresholdDictionnary(string name, Dictionary<uint, long> thresholds)
        {
            m_name = name;
            m_levels = thresholds;
        }

        public string Name
        {
            get { return m_name; }
        }

        public long GetLowerBound(uint level)
        {
            if (m_levels.ContainsKey(level))
                return m_levels[level];
            throw new Exception("Level " + level + " not found in " + Name + " threshold");
        }

        public long GetLowerBound(long experience)
        {
            return GetLowerBound(GetLevel(experience));
        }

        public long GetUpperBound(uint level)
        {
            return GetLowerBound(level + 1);
        }

        public long GetUpperBound(long experience)
        {
            return GetUpperBound(GetLevel(experience));
        }

        public uint GetLevel(long experience)
        {
            try
            {
                return m_levels.First(l => l.Value > experience).Key - 1;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(
                    string.Format("Experience {0} not bind to a level in {1} threshold", experience, Name), ex);
            }
        }
    }
}