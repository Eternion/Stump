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
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Manager;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors.Actor;

namespace Stump.Server.WorldServer.Entities
{
    public partial class Character : Actor, IInventoryOwner, ISpellsOwner, IAligned
    {

        public Character(WorldClient client, CharacterRecord record)
            : base(record.Id, record.Name, CharacterManager.GetStuffedCharacterLook(record), new VectorIsometric(World.World.Instance.GetMap(record.MapId), record.CellId, record.Direction))
        {
            Client = client;
            Record = record;
            Breed = (BreedEnum)record.Breed;
            Sex = (SexTypeEnum)record.Sex;
            Energy = record.Energy;
            EnergyMax = record.EnergyMax;
            Experience = record.Experience;
            Inventory = new Inventory(record.Inventory, this);
            SpellInventory = new SpellInventory(this, record.Spells, record.SpellsPoints);
            Restrictions = new CharacterRestrictions(record.Restrictions);
            Alignment = new ActorAlignment(this,record.Alignment);
            TitleId = record.TitleId;
            TitleParam = record.TitleParam;
        }


        public void Save()
        {
            //TODO : revert loading, SaveAndFlush record
        }

        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            return new GameRolePlayCharacterInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, new HumanInformations(FollowingCharactersLook, EmoteId, EmoteEndTime, Restrictions.ToActorRestrictionsInformations(), TitleId, TitleParam), Alignment.ToActorAlignmentInformations());
        }

    }
}