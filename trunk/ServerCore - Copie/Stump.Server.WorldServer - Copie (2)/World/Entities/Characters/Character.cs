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
using System.Linq;
using Stump.BaseCore.Framework.Utils;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Helpers;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Storages;

namespace Stump.Server.WorldServer.World.Entities.Characters
{
    public partial class Character : Actor, IInventoryOwner, ISpellsOwner, IAligned
    {

        public Character(WorldClient client, CharacterRecord record)
            : base(record.Id, record.Name, CharacterManager.GetStuffedCharacterLook(record), new VectorIsometric(Singleton<World>.Instance.GetMap(record.MapId), record.CellId, record.Direction))
        {
            Client = client;
            m_record = record;
            Breed = record.Breed;
            Sex = record.Sex;
            Energy = record.Energy;
            EnergyMax = record.EnergyMax;
            Experience = record.Experience;
            Inventory = new Inventory(this, record.Inventory);
            SpellInventory = new SpellInventory(this, record.Spells, record.SpellsPoints);
            Restrictions = new CharacterRestrictions(record.Restrictions);
            Alignment = new ActorAlignment(this,record.Alignment);
            TitleId = record.TitleId;
            TitleParam = record.TitleParam;
        }


        public void Save()
        {
            m_record.Breed = Breed;
            m_record.Sex = Sex;
            m_record.Energy = Energy;
            m_record.EnergyMax = EnergyMax;
            m_record.Experience = Experience;
            m_record.Inventory = Inventory.Record;
            //Inventory.Save();
            //spells
            m_record.Alignment = Alignment.Record;
            Alignment.Save();
            m_record.TitleId = TitleId;
            m_record.TitleParam=TitleParam;
            m_record.SaveAndFlush();
        }


        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            return new GameRolePlayCharacterInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, new HumanInformations(FollowingCharacters.Select(f => f.Look.EntityLook).ToList(), EmoteId, EmoteEndTime, Restrictions.ToActorRestrictionsInformations(), TitleId, TitleParam), Alignment.ToActorAlignmentInformations());
        }
    }
}