// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using NLog;
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("characters")]
    public sealed class CharacterRecord : ActiveRecordBase<CharacterRecord>
    {
        private static readonly IdGenerator IdGenerator = new IdGenerator(typeof (CharacterRecord), "Id");

        /// <summary>
        ///   Logger for this class.
        /// </summary>
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Indicate if this record just got created.
        /// </summary>
        public bool New;



        private List<int> m_colors;
        private string m_dbColors;

        private string m_dbSkins;
        private List<uint> m_skins;



        /// <summary>
        ///   Constructor
        /// </summary>
        public CharacterRecord()
        {
            m_colors = new List<int>();
            m_skins = new List<uint>();
            Spells = new Dictionary<uint, SpellRecord>();
        }

        /// <summary>
        ///   Account of the character
        /// </summary>
        public AccountRecord Account
        {
            get;
            set;
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("Name", Length = 18)]
        public string Name
        {
            get;
            set;
        }

        [Property("Level")]
        public int Level
        {
            get;
            set;
        }

        [Property("Colors")]
        private string DBColors
        {
            get { return m_dbColors; }
            set
            {
                m_dbColors = value;

                if (m_colors.Count <= 0)
                    UpdateColors();
            }
        }

        public List<int> Colors
        {
            get { return m_colors ?? (m_colors = DBColors.Trim().Split(',').Select(entry => int.Parse(entry)).ToList()); }
            set
            {
                m_colors = value;

                UpdateColors();
            }
        }

        public List<int> ColorsIndexed
        {
            get
            {
                return
                    Colors.Select(
                        (color, index) => int.Parse((index + 1) + color.ToString("X6"), NumberStyles.HexNumber)).ToList();
            }
        }


        [Property("Breed")]
        public int Breed
        {
            get;
            set;
        }

        [Property("SexId")]
        public int SexId
        {
            get;
            set;
        }

        [Property("Skins", NotNull = true)]
        private string DBSkins
        {
            get { return m_dbSkins; }
            set
            {
                m_dbSkins = value;

                if (m_skins.Count <= 0)
                    UpdateSkins();
            }
        }

        public List<uint> Skins
        {
            get { return m_skins ?? (m_skins = DBSkins.Trim().Split(',').Select(entry => uint.Parse(entry)).ToList()); }
            set
            {
                m_skins = value;

                UpdateSkins();
            }
        }

        [Property("Scale")]
        public int Scale
        {
            get;
            set;
        }

        [Property("Kamas")]
        public long Kamas
        {
            get;
            set;
        }

        [Property("MapId")]
        public int MapId
        {
            get;
            set;
        }

        [Property("CellId")]
        public ushort CellId
        {
            get;
            set;
        }

        [Property("Direction")]
        public DirectionsEnum Direction
        {
            get;
            set;
        }

        [Property("BaseHealth", NotNull = true)]
        public int BaseHealth
        {
            get;
            set;
        }

        [Property("DamageTaken", NotNull = true)]
        public int DamageTaken
        {
            get;
            set;
        }

        [Property("Strength", NotNull = true)]
        public int Strength
        {
            get;
            set;
        }

        [Property("Chance", NotNull = true)]
        public int Chance
        {
            get;
            set;
        }

        [Property("Vitality", NotNull = true)]
        public int Vitality
        {
            get;
            set;
        }

        [Property("Wisdom", NotNull = true)]
        public int Wisdom
        {
            get;
            set;
        }

        [Property("Intelligence", NotNull = true)]
        public int Intelligence
        {
            get;
            set;
        }

        [Property("Agility", NotNull = true)]
        public int Agility
        {
            get;
            set;
        }

        [Property("StatsPoints", NotNull = true)]
        public int StatsPoints
        {
            get;
            set;
        }

        [Property("SpellsPoints", NotNull = true)]
        public int SpellsPoints
        {
            get;
            set;
        }

        #region Spell

        public IDictionary<uint, SpellRecord> Spells
        {
            get;
            private set;
        }

        /// <summary>
        ///   Add a new spell to character which register directly in database.
        /// </summary>
        public void AddSpell(uint spellId, int position, int level)
        {
            if (Spells != null)
            {
                if (Spells.ContainsKey(spellId))
                {
                    logger.Error("Spell ({0}, Id: {1}) added twice to Character {2} (Breed: {3})",
                                 (SpellIdEnum) spellId,
                                 spellId,
                                 this,
                                 (BreedEnum) Breed);
                }

                var record = new SpellRecord(spellId, (uint) Id, position, level);
                Spells.Add(spellId, record);
                record.Save();
            }
        }

        /// <summary>
        ///   Add a new spell to character which register directly in database.
        /// </summary>
        public void AddSpell(SpellIdEnum spellId, int position, int level)
        {
            if (Spells != null)
            {
                if (Spells.ContainsKey((uint) spellId))
                {
                    logger.Error("Spell ({0}, Id: {1}) added twice to Character {2} (Breed: {3})",
                                 spellId,
                                 spellId,
                                 this,
                                 (BreedEnum) Breed);
                }

                var record = new SpellRecord((uint) spellId, (uint) Id, position, level);
                Spells.Add((uint) spellId, record);
                record.Save();
            }
        }

        public bool RemoveSpell(uint id)
        {
            SpellRecord spell;
            if (Spells.TryGetValue(id, out spell))
            {
                return RemoveSpell(spell);
            }
            return false;
        }

        private bool RemoveSpell(SpellRecord record)
        {
            record.Delete();
            Spells.Remove(record.SpellId);
            return true;
        }

        public void ModifySpellPosition(int spellid, int newpos)
        {
            if (Spells.ContainsKey((uint) spellid))
            {
                Spells[(uint) spellid].Position = newpos;
            }
        }

        public void SaveSpells()
        {
            foreach (SpellRecord sr in Spells.Values)
            {
                sr.Save();
            }
        }

        public void LoadSpells()
        {
            if (!New)
            {
                Spells = new Dictionary<uint, SpellRecord>();
                SpellRecord[] dbSpells = SpellRecord.FindAll(Restrictions.Eq("OwnerId", (uint) Id));
                foreach (SpellRecord spell in dbSpells)
                {
                    try
                    {
                        Spells.Add(spell.SpellId, spell);
                    }
                    catch (Exception e)
                    {
                        string msg = string.Format("Spell {0} of {1} was fetched twice from DB.", spell.SpellId, this);
                        logger.ErrorException(msg, e);
                    }
                }
            }
        }

        #endregion

        #region Delete

        public void DeleteAssociatedRecords()
        {
            SpellRecord.DeleteAll("OwnerId = " + Id);
            CharacterItemRecord.DeleteAll("OwnerId = " + Id);
        }

        #endregion

        public static long GetNextId()
        {
            return IdGenerator.Next();
        }

        public void UpdateColors()
        {
            if (m_colors.Count > 1)
                DBColors = string.Join(",", m_colors.Select(entry => entry.ToString()));
            else
                m_colors = DBColors.Trim().Split(',').Select(entry => int.Parse(entry)).ToList();
        }

        public void UpdateSkins()
        {
            if (m_skins.Count > 0)
                DBSkins = string.Join(",", m_skins.Select(entry => entry.ToString()));
            else
                m_skins = DBSkins.Trim().Split(',').Select(entry => uint.Parse(entry)).ToList();
        }

        /// <summary>
        ///   Find a character by his name
        /// </summary>
        /// <param name = "characterName">name</param>
        /// <returns></returns>
        public static CharacterRecord FindCharacterByName(string characterName)
        {
            return FindOne(Restrictions.Eq("Name", characterName));
        }

        /// <summary>
        ///   True if the name exist else false
        /// </summary>
        /// <param name = "name"></param>
        /// <returns></returns>
        public static bool IsNameExists(string name)
        {
            return Exists(Restrictions.Eq("Name", name));
        }

        /// <summary>
        ///   Find a character by his id
        /// </summary>
        /// <param name = "characterId"></param>
        /// <returns></returns>
        public static CharacterRecord FindCharacterById(int characterId)
        {
            return FindByPrimaryKey(characterId);
        }

        public static int GetCount()
        {
            return Count();
        }

        public override void Save()
        {
            if (New)
                Create();
            else
                Update();
        }

        public override void SaveAndFlush()
        {
            if (New)
                CreateAndFlush();
            else
                UpdateAndFlush();
        }

        protected override bool BeforeSave(IDictionary state)
        {
            UpdateColors();
            UpdateSkins();

            return base.BeforeSave(state);
        }
    }
}