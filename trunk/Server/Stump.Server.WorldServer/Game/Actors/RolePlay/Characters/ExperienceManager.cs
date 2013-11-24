using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public class ExperienceManager : DataManager<ExperienceManager>
    {
        private readonly Dictionary<byte, ExperienceTableEntry> m_records = new Dictionary<byte, ExperienceTableEntry>();
        private KeyValuePair<byte, ExperienceTableEntry> m_highestCharacterLevel;
        private KeyValuePair<byte, ExperienceTableEntry> m_highestGrade;
        private KeyValuePair<byte, ExperienceTableEntry> m_highestGuildLevel;

        public byte HighestCharacterLevel
        {
            get { return m_highestCharacterLevel.Key; }
        }

        public byte HighestGuildLevel
        {
            get { return m_highestGuildLevel.Key; }
        }

        public byte HighestGrade
        {
            get { return m_highestGrade.Key; }
        }

        #region Character

        /// <summary>
        ///     Get the experience requiered to access the given character level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public long GetCharacterLevelExperience(byte level)
        {
            if (m_records.ContainsKey(level))
            {
                long? exp = m_records[level].CharacterExp;

                if (!exp.HasValue)
                    throw new Exception("Character level " + level + " is not defined");

                return exp.Value;
            }

            throw new Exception("Level " + level + " not found");
        }

        /// <summary>
        ///     Get the experience to reach the next character level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public long GetCharacterNextLevelExperience(byte level)
        {
            if (m_records.ContainsKey((byte) (level + 1)))
            {
                long? exp = m_records[(byte) (level + 1)].CharacterExp;

                if (!exp.HasValue)
                    throw new Exception("Character level " + level + " is not defined");

                return exp.Value;
            }
            else
            {
                return long.MaxValue;
            }
        }

        public byte GetCharacterLevel(long experience)
        {
            try
            {
                if (experience >= m_highestCharacterLevel.Value.CharacterExp)
                    return m_highestCharacterLevel.Key;

                return (byte) (m_records.First(entry => entry.Value.CharacterExp > experience).Key - 1);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(string.Format("Experience {0} isn't bind to a character level", experience), ex);
            }
        }

        #endregion

        #region Alignement

        /// <summary>
        ///     Get the honor requiered to access the given grade
        /// </summary>
        /// <returns></returns>
        public ushort GetAlignementGradeHonor(byte grade)
        {
            if (m_records.ContainsKey(grade))
            {
                ushort? honor = m_records[grade].AlignmentHonor;

                if (!honor.HasValue)
                    throw new Exception("Grade " + grade + " is not defined");

                return honor.Value;
            }

            throw new Exception("Grade " + grade + " not found");
        }

        /// <summary>
        ///     Get the honor to reach the next grade
        /// </summary>
        /// <returns></returns>
        public ushort GetAlignementNextGradeHonor(byte grade)
        {
            if (!m_records.ContainsKey((byte) (grade + 1)))
                return (ushort)short.MaxValue;
                

            var honor = m_records[(byte) (grade + 1)].AlignmentHonor;

            return !honor.HasValue ? (ushort)short.MaxValue : honor.Value;
        }

        public byte GetAlignementGrade(ushort honor)
        {
            try
            {
                if (honor >= m_highestGrade.Value.AlignmentHonor)
                    return m_highestGrade.Key;

                return (byte) (m_records.First(entry => entry.Value.AlignmentHonor > honor).Key - 1);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(string.Format("Honor {0} isn't bind to a grade", honor), ex);
            }
        }

        #endregion

        #region Guild

        /// <summary>
        ///     Get the experience requiered to access the given guild level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public long GetGuildLevelExperience(byte level)
        {
            if (m_records.ContainsKey(level))
            {
                long? exp = m_records[level].GuildExp;

                if (!exp.HasValue)
                    throw new Exception("Guild level " + level + " is not defined");

                return exp.Value;
            }

            throw new Exception("Level " + level + " not found");
        }

        /// <summary>
        ///     Get the experience to reach the next guild level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public long GetGuildNextLevelExperience(byte level)
        {
            if (m_records.ContainsKey((byte) (level + 1)))
            {
                long? exp = m_records[(byte) (level + 1)].GuildExp;

                if (!exp.HasValue)
                    throw new Exception("Guild level " + level + " is not defined");

                return exp.Value;
            }
            else
            {
                return long.MaxValue;
            }
        }

        public byte GetGuildLevel(long experience)
        {
            try
            {
                if (experience >= m_highestGuildLevel.Value.GuildExp)
                    return m_highestGuildLevel.Key;

                return (byte) (m_records.First(entry => entry.Value.GuildExp > experience).Key - 1);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(string.Format("Experience {0} isn't bind to a guild level", experience), ex);
            }
        }

        #endregion

        [Initialization(InitializationPass.Fourth)]
        public override void Initialize()
        {
            foreach (
                ExperienceTableEntry record in Database.Query<ExperienceTableEntry>(ExperienceTableRelator.FetchQuery))
            {
                if (record.Level > 200)
                    throw new Exception("Level cannot exceed 200 (protocol constraint)");

                m_records.Add((byte) record.Level, record);
            }

            m_highestCharacterLevel = m_records.OrderByDescending(entry => entry.Value.CharacterExp).FirstOrDefault();
            m_highestGrade = m_records.OrderByDescending(entry => entry.Value.AlignmentHonor).FirstOrDefault();
            m_highestGuildLevel = m_records.OrderByDescending(entry => entry.Value.GuildExp).FirstOrDefault();
        }
    }
}