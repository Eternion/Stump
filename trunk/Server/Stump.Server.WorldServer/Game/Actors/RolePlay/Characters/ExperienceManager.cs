using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public class ExperienceManager : Singleton<ExperienceManager>
    {
        private readonly Dictionary<byte, ExperienceRecord> m_records = new Dictionary<byte, ExperienceRecord>();
        private KeyValuePair<byte, ExperienceRecord> m_highestCharacterLevel;
        private KeyValuePair<byte, ExperienceRecord> m_highestGrade;

        public byte HighestCharacterLevel
        {
            get { return m_highestCharacterLevel.Key; }
        }

        public byte HighestGrade
        {
            get { return m_highestGrade.Key; }
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
                long? exp = m_records[level].CharacterExp;

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
        /// Get the honor requiered to access the given grade
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
        /// Get the honor to reach the next grade
        /// </summary>
        /// <returns></returns>
        public ushort GetAlignementNextGradeHonor(byte grade)
        {
            if (m_records.ContainsKey((byte) (grade + 1)))
            {
                ushort? honor = m_records[(byte)( grade + 1 )].AlignmentHonor;

                if (!honor.HasValue)
                    throw new Exception("Grade " + grade + " is not defined");

                return honor.Value;
            }
            else
            {
                return ushort.MaxValue;
            }
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

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            foreach (ExperienceRecord record in ExperienceRecord.FindAll())
            {
                m_records.Add(record.Level, record);
            }

            m_highestCharacterLevel = m_records.OrderByDescending(entry => entry.Value.CharacterExp).FirstOrDefault();
            m_highestGrade = m_records.OrderByDescending(entry => entry.Value.AlignmentHonor).FirstOrDefault();
        }
    }
}