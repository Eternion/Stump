using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters
{
    public class ExperienceManager : Singleton<ExperienceManager>
    {
        private readonly Dictionary<byte, ExperienceRecord> m_records = new Dictionary<byte, ExperienceRecord>();
        private KeyValuePair<byte, ExperienceRecord> m_highestLevel;

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            foreach (var record in ExperienceRecord.FindAll())
            {
                m_records.Add(record.Level, record);
            }

            m_highestLevel = m_records.OrderByDescending(entry => entry.Key).FirstOrDefault();
        }

        public byte HighestLevel
        {
            get { return m_highestLevel.Key; }
        }

        #region Character

        /// <summary>
        /// Get the experience requiered to access the given character level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public long GetCharacterLevelExperience(byte level)
        {
            if (m_records.ContainsKey(level))
            {
                var exp = m_records[level].CharacterExp;

                if (!exp.HasValue)
                    throw new Exception("Character level " + level + " is not defined");

                return exp.Value;
            }

            throw new Exception("Level " + level + " not found");
        }

        /// <summary>
        /// Get the experience to reach the next character level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public long GetCharacterNextLevelExperience(byte level)
        {
            if (m_records.ContainsKey((byte) (level + 1)))
            {
                var exp = m_records[(byte) (level + 1)].CharacterExp;

                if (!exp.HasValue)
                    throw new Exception("Character level " + level + " is not defined");

                return exp.Value;
            }

            throw new Exception("Level " + level + " not found");
        }

        public byte GetCharacterLevel(long experience)
        {
            try
            {
                if (experience >= m_highestLevel.Value.CharacterExp)
                    return m_highestLevel.Key;

                return (byte) (m_records.First(entry => entry.Value.CharacterExp > experience).Key - 1);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(string.Format("Experience {0} isn't bind to a character level", experience), ex);
            }
        }

        #endregion
    }
}