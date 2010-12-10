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
using Castle.ActiveRecord;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public partial class Character : Entity, IMovable
    {
        /// <summary>
        ///   Constructor called when a character has been successfully selected.
        /// </summary>
        /// <param name = "cr"></param>
        /// <param name = "client"></param>
        public Character(CharacterRecord cr, WorldClient client)
        {
            Record = cr;
            Client = client;

            Id = cr.Id;
            Name = cr.Name;
            Level = cr.Level;
            BreedId = (BreedEnum) cr.Classe;
            Sex = (SexTypeEnum) cr.SexId;
            Kamas = cr.Kamas;
            BaseHealth = cr.BaseHealth;
            DamageTaken = cr.DamageTaken;
            StatsPoint = cr.StatsPoints;
            SpellsPoints = cr.SpellsPoints;
            EmoteId = 0;

            Map = World.Instance.Maps[cr.MapId];
            CellId = cr.CellId;
            Position = Location.Empty;
            InWorld = false;


            // -> entity look
            Skins = cr.Skins;
            Colors = cr.Colors;
            Scale = cr.Scale;
            BonesId = 1;

            Inventory = new Inventory(this);
            Inventory.LoadInventory();

            Stats = new StatsFields(this);
            Stats["Strength"].Base = cr.Strength;
            Stats["Vitality"].Base = cr.Vitality;
            Stats["Wisdom"].Base = cr.Wisdom;
            Stats["Intelligence"].Base = cr.Intelligence;
            Stats["Chance"].Base = cr.Chance;
            Stats["Agility"].Base = cr.Agility;

            cr.LoadSpells();
            Spells = new SpellCollection(this);
            foreach (SpellRecord sr in cr.Spells.Values)
            {
                Spells.AddSpell(SpellManager.GetSpell(sr.SpellId));
                Spells[sr.SpellId].CurrentLevel = sr.Level;
                Spells[sr.SpellId].Position = sr.Position;
            }

            Zone = World.Instance.RetrieveZoneByMapId(Map.Id);
        }

        /// <summary>
        ///   Spawn the character on the map. It can be called once.
        /// </summary>
        public void FirstSpawnCharacter()
        {
            if (!InWorld)
            {
                Map.OnEnter(this);

                InWorld = true;
                World.Instance.AddCharacter(this);
            }
        }

        /// <summary>
        ///   Custom change map function.
        ///   Currently used ONLY on teleportation command.
        /// </summary>
        /// <param name = "mapid"></param>
        /// <returns></returns>
        public bool ChangeMap(int mapid)
        {
            if (!World.Instance.Maps.ContainsKey(mapid))
            {
                return false;
            }

            Client.ActiveCharacter.NextMap = World.Instance.Maps[mapid];
            Map.OnLeave(this);

            Map = Client.ActiveCharacter.NextMap;
            MapHandler.SendCurrentMapMessage(Client, mapid);
            Map.OnEnter(this);

            return true;
        }

        public void LogOut()
        {
            if (InWorld)
            {
                Inventory.UnLoadInventory();
                SaveNow();

                if (Map != null)
                    Map.OnLeave(this);

                World.Instance.RemoveCharacter(this);
                InWorld = false;
            }
        }

        public void SendChatMessage(string msg, string from)
        {
            ChatHandler.SendCustomServerMessage(Client, StringUtils.HtmlEntities(msg), from);
        }

        /// <summary>
        ///   Send a packet to this character.
        /// </summary>
        public void SendPacket(Message message)
        {
            Client.Send(message);
        }

        public void SetKamas(int amount)
        {
            Kamas = amount;
            CharacterHandler.SendKamasUpdateMessage((Client));
        }

        #region Save

        public void SaveLater()
        {
            World.Instance.TaskPool.EnqueueTask(SaveNow);
        }

        /// <summary>
        ///   Save character's fields to database.
        ///   This is not a Save() from A-R, we don't want to Save a new character but update his data.
        /// </summary>
        public void SaveNow()
        {
            m_record.Kamas = Kamas;
            m_record.Level = Level;

            if (Map != null)
                m_record.MapId = Map.Id;

            m_record.CellId = CellId;
            m_record.BaseHealth = BaseHealth;
            m_record.DamageTaken = DamageTaken;
            m_record.StatsPoints = StatsPoint;
            m_record.SpellsPoints = SpellsPoints;

            if (Stats != null)
            {
                m_record.Strength = Stats["Strength"].Base;
                m_record.Vitality = Stats["Vitality"].Base;
                m_record.Wisdom = Stats["Wisdom"].Base;
                m_record.Intelligence = Stats["Intelligence"].Base;
                m_record.Chance = Stats["Chance"].Base;
                m_record.Agility = Stats["Agility"].Base;
            }

            try
            {
                using (var saveScope = new SessionScope(FlushAction.Never))
                {
                    foreach (Item item in Inventory.Items)
                    {
                        item.SaveNow();
                    }

                    m_record.SaveSpells();

                    m_record.Save();

                    saveScope.Flush();
                }
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred while Saving character {0}. {1}", Name, e.Message);
                m_record.Save();
            }
        }

        #endregion

        #region Movements

        /// <summary>
        ///   Indicate or set if entity is moving.
        /// </summary>
        public bool IsMoving
        {
            get;
            set;
        }

        public void Jump(Location to)
        {
            throw new NotImplementedException("You can't jump with your character yet.");
        }

        public void Move()
        {
            throw new NotImplementedException("You can't {really} move with your character yet.");
        }

        public void Stop(bool b)
        {
            // todo
        }

        #endregion

        #region Spells

        /// <summary>
        ///   Add a new freshly created spell to this character.
        ///   It's also create a record.
        /// </summary>
        /// <param name = "spell"></param>
        public void AddSpell(Spell spell)
        {
            Spells.AddSpell(spell);
            m_record.AddSpell((uint) spell.Id, spell.Position, spell.CurrentLevel);
        }

        public void ModifySpellPos(SpellIdEnum spellId, int newPos)
        {
            Spells.MoveSpell(spellId, newPos);
            m_record.ModifySpellPosition((int) spellId, newPos);
        }

        public void RemoveSpell(SpellIdEnum spellid)
        {
            // todo
        }

        #endregion
    }
}