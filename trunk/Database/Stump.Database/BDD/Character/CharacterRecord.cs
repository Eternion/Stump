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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("characters")]
    public sealed class CharacterRecord : ActiveRecordBase<CharacterRecord>
    {

        public bool New;

        public CharacterRecord()
        {
            Spells = new Dictionary<uint, CharacterSpellRecord>();
        }

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

        [Property("Level", NotNull=true, Default="1")]
        public int Level
        {
            get;
            set;
        }

        [Property("Breed")]
        public int Breed
        {
            get;
            set;
        }

        [Property("SexId", NotNull=true)]
        public int SexId
        {
            get;
            set;
        }

        [Property("BonesId", NotNull = true, Default = "1")]
        private uint BonesId
        {
            get;
            set;
        }

        [Property("Skins", NotNull = true)]
        private string Skins
        {
            get;
            set;
        }

        [Property("Color1", NotNull = true)]
        private int Color1
        {
            get;
            set;
        }

        [Property("Color2", NotNull = true)]
        private int Color2
        {
            get;
            set;
        }

        [Property("Color3", NotNull = true)]
        private int Color3
        {
            get;
            set;
        }

        [Property("Color4", NotNull = true)]
        private int Color4
        {
            get;
            set;
        }

        [Property("Color5", NotNull = true)]
        private int Color5
        {
            get;
            set;
        }

        [Property("Height", NotNull = true, Default="130")]
        private int Height
        {
            get;
            set;
        }

        [Property("Width", NotNull = true, Default="130")]
        private int Width
        {
            get;
            set;
        }

        [Property("Kamas", NotNull=true, Default="0")]
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


        #region Stats

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

        #endregion

        #region Spell

        public IDictionary<uint, CharacterSpellRecord> Spells
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
              //  if (Spells.ContainsKey(spellId))
                //{
                //    logger.Error("Spell ({0}, Id: {1}) added twice to Character {2} (Breed: {3})",
                //                 (SpellIdEnum)spellId,
                //                 spellId,
                //                 this,
                //                 (PlayableBreedEnum)Breed);
               //}

                var record = new CharacterSpellRecord(spellId, this, position, level);
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
                if (Spells.ContainsKey((uint)spellId))
                {
                    //logger.Error("Spell ({0}, Id: {1}) added twice to Character {2} (Breed: {3})",
                    //             spellId,
                    //             spellId,
                    //             this,
                    //             (PlayableBreedEnum)Breed);
                }

                var record = new CharacterSpellRecord((uint)spellId, this, position, level);
                Spells.Add((uint)spellId, record);
                record.Save();
            }
        }

        public bool RemoveSpell(uint id)
        {
            CharacterSpellRecord spell;
            if (Spells.TryGetValue(id, out spell))
            {
                return RemoveSpell(spell);
            }
            return false;
        }

        private bool RemoveSpell(CharacterSpellRecord record)
        {
            record.Delete();
            Spells.Remove(record.SpellId);
            return true;
        }

        public void ModifySpellPosition(int spellid, int newpos)
        {
            if (Spells.ContainsKey((uint)spellid))
            {
                Spells[(uint)spellid].Position = newpos;
            }
        }

        public void SaveSpells()
        {
            foreach (CharacterSpellRecord sr in Spells.Values)
            {
                sr.Save();
            }
        }

        public void LoadSpells()
        {
            if (!New)
            {
                Spells = new Dictionary<uint, CharacterSpellRecord>();
                CharacterSpellRecord[] dbSpells = CharacterSpellRecord.FindAll(Restrictions.Eq("OwnerId", (uint)Id));
                foreach (CharacterSpellRecord spell in dbSpells)
                {
                    try
                    {
                        Spells.Add(spell.SpellId, spell);
                    }
                    catch (Exception e)
                    {
                        string msg = string.Format("Spell {0} of {1} was fetched twice from DB.", spell.SpellId, this);
                      //  logger.ErrorException(msg, e);
                    }
                }
            }
        }

        #endregion

        #region Delete

        public void DeleteAssociatedRecords()
        {
            CharacterSpellRecord.DeleteAll("OwnerId = " + Id);
        //    CharacterItemRecord.DeleteAll("OwnerId = " + Id);
        }

        #endregion

    }
}