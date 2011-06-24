using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.WorldServer.Threshold
{
    public class ThresholdDictionnary
    {
        private Dictionary<uint, long> m_levels = new Dictionary<uint,long>();

        public ThresholdDictionnary(string name, XDocument doc)
        {
            Name = name;

            Init(doc);
        }

        public string Name
        {
            get;
            private set;
        }

        private void Init(XDocument doc)
        {
            var levels = doc.Element("Levels");

            if (levels == null)
                throw new Exception("Cannot find the node 'Levels'");

            foreach (var level in levels.Elements())
            {
                string[] name = level.Name.ToString().Split('_');

                if (name.Length != 2)
                    throw new Exception("Malformated Threshold xml");

                uint key;
                long value;
                if (uint.TryParse(name[1], out key) && long.TryParse(level.Value, out value) && !m_levels.ContainsKey(key))
                {
                    m_levels.Add(key, value);
                }
                else
                    throw new Exception("Malformated Threshold xml");
            }
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
                throw new Exception(string.Format("Experience {0} not bind to a level in {1} threshold", experience, Name), ex);
            }
        }
    }
}
