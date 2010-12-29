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
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;
using Stump.Server.WorldServer.Manager;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Manager;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Spells;
using Item = Stump.Server.WorldServer.Items.Item;

namespace Stump.Server.WorldServer.Entities
{
    public partial class Character : LivingEntity, IPacketReceiver
    {
        /// <summary>
        ///   Constructor called when a character has been successfully selected.
        /// </summary>
        /// <param name = "record"></param>
        /// <param name = "client"></param>
        public Character(CharacterRecord record, WorldClient client)
            : base(record.Id)
        {
            Record = record;
            Client = client;

            Name = record.Name;
            Level = record.Level;
            BreedId = (PlayableBreedEnum)record.Breed;
            Sex = (SexTypeEnum) record.SexId;
            Kamas = record.Kamas;
            StatsPoint = record.StatsPoints;
            SpellsPoints = record.SpellsPoints;
            EmoteId = 0;

            Position = new VectorIsometric(World.Instance.Maps[record.MapId], record.CellId, record.Direction);
            InWorld = false;

            // -> entity look
            Look = CharacterManager.GetStuffedCharacterLook(record);

            Inventory = new Inventory(this);
            Inventory.LoadInventory();
            
            Stats = new StatsFields(this);
            Stats["Strength"].Base = record.Strength;
            Stats["Vitality"].Base = record.Vitality;
            Stats["Wisdom"].Base = record.Wisdom;
            Stats["Intelligence"].Base = record.Intelligence;
            Stats["Chance"].Base = record.Chance;
            Stats["Agility"].Base = record.Agility;

            record.LoadSpells();
            Spells = new SpellCollection(this);
            foreach (SpellRecord sr in record.Spells.Values)
            {
                Spells.AddSpell(SpellManager.GetSpell(sr.SpellId));
                Spells[sr.SpellId].CurrentLevel = sr.Level;
                Spells[sr.SpellId].Position = sr.Position;
            }
        }

        /// <summary>
        ///   Send a packet to this character.
        /// </summary>
        public void Send(Message message)
        {
            Client.Send(message);
        }

        public void LogOut()
        {
            if (InWorld)
            {
                Inventory.UnLoadInventory();
                SaveNow();

                if (Map != null)
                    Map.RemoveEntity(this);

                World.Instance.RemoveCharacter(this);
                InWorld = false;
            }
        }

        /// <summary>
        ///   Spawn the character on the map. It can be called once.
        /// </summary>
        public void FirstSpawn()
        {
            if (!InWorld)
            {
                Position.Map.AddEntity(this);

                InWorld = true;
                World.Instance.AddCharacter(this);
            }
        }

        public void ChangeMap(Map nextMap)
        {
            if (IsMoving)
                MovementEnded();

            Map lastMap = Map;

            NextMap = nextMap;
            Map.RemoveEntity(this);

            var neighbour = lastMap.GetMapNeighbourByMapid(nextMap.Id);

            var cellId = Position.CellId;
            if (neighbour != MapNeighbour.None)
            {
                cellId = Map.GetCellAfterChangeMap(Position.CellId, neighbour);
            }

            Position.ChangeLocation(nextMap, cellId);

            Map.AddEntity(this);

            NotifyChangeMap(lastMap);
        }


        public void ChangeMap(Map nextMap, ushort cellId)
        {
            Map lastMap = Map;

            NextMap = nextMap;
            Map.RemoveEntity(this);

            Position.ChangeLocation(nextMap, cellId);

            Map.AddEntity(this);

            NotifyChangeMap(lastMap);
        }

        public void AddKamas(long amount)
        {
            SetKamas(Kamas + amount);
        }

        public void SubKamas(long amount)
        {
            SetKamas(Kamas - amount);
        }

        public void SetKamas(long amount)
        {
            Kamas = amount;
            InventoryHandler.SendKamasUpdateMessage(Client, amount);
        }

        public override GameRolePlayActorInformations ToNetworkActor(WorldClient client)
        {
            return new GameRolePlayCharacterInformations(
                (int) Id,
                Look.EntityLook,
                GetEntityDisposition(),
                Name,
                GetHumanInformations(),
                GetActorAlignmentInformations());
        }

        public override FightTeamMemberInformations ToNetworkTeamMember()
        {
            if (!IsInFight)
                return null;

            return new FightTeamMemberCharacterInformations(
                (int) Id,
                Name,
                (uint) Level);
        }

        public override GameFightFighterInformations ToNetworkFighter()
        {
            if (!IsInFight)
                return null;

            return new GameFightCharacterInformations(
                (int)Id,
                Look.EntityLook,
                CurrentFighter.GetEntityDisposition(),
                (uint)( (FightGroup)Group ).TeamId,
                !( CurrentFighter.IsDead || CurrentFighter.IsReady ),
                CurrentFighter.GetFightMinimalStats(),
                Name,
                (uint)Level,
                GetActorAlignmentInformations());
        }

        public CharacterBaseInformations GetBaseInformations()
        {
            return new CharacterBaseInformations(
                (uint) Id,
                (uint) Level,
                Name,
                Look.EntityLook,
                (int) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE);
        }

        // todo : complete this

        public HumanInformations GetHumanInformations()
        {
            return new HumanInformations(
                new List<EntityLook>(),
                0,
                0,
                new ActorRestrictionsInformations(),
                0,
                "");
        }

        // todo : complete this

        public ActorAlignmentInformations GetActorAlignmentInformations()
        {
            return new ActorAlignmentInformations(
                0,
                0,
                0,
                0,
                0);
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
            Record.Kamas = Kamas;
            Record.Level = Level;

            if (Map != null)
                Record.MapId = Position.Map.Id;

            Record.CellId = Position.CellId;
            Record.Direction = Position.Direction;
            Record.BaseHealth = BaseHealth;
            Record.DamageTaken = DamageTaken;
            Record.StatsPoints = StatsPoint;
            Record.SpellsPoints = SpellsPoints;

            if (Stats != null)
            {
                Record.Strength = Stats["Strength"].Base;
                Record.Vitality = Stats["Vitality"].Base;
                Record.Wisdom = Stats["Wisdom"].Base;
                Record.Intelligence = Stats["Intelligence"].Base;
                Record.Chance = Stats["Chance"].Base;
                Record.Agility = Stats["Agility"].Base;
            }

            try
            {
                using (var saveScope = new SessionScope(FlushAction.Never))
                {
                    foreach (Item item in Inventory.Items)
                    {
                        item.SaveNow();
                    }

                    Record.SaveSpells();

                    Record.Save();

                    saveScope.Flush();
                }
            }
            catch (Exception e)
            {
                logger.Error("Exception occurred while Saving character {0}. {1}", Name, e.Message);
                Record.Save();
            }
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
            Record.AddSpell((uint)spell.Id, spell.Position, spell.CurrentLevel);
        }

        public void ModifySpellPos(SpellIdEnum spellId, int newPos)
        {
            Spells.MoveSpell(spellId, newPos);
            Record.ModifySpellPosition((int)spellId, newPos);
        }

        public void RemoveSpell(SpellIdEnum spellid)
        {
            // todo
        }

        #endregion
    }
}