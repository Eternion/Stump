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
using Stump.BaseCore.Framework.Utils;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.DataProvider.Data.TaxCollector;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Storages;

namespace Stump.Server.WorldServer.World.Entities.TaxCollectors
{
    public class TaxCollector : Actor, IInventoryOwner, ITrader,IAttackable
    {

        protected TaxCollector(Guild guild,long id, ExtendedLook look, VectorIsometric position, Inventory inventory)
            :base(id,"",look,position)
        {
            var fn = TaxCollectorFirstNameManager.Instance.Get();
            var n = TaxCollectorDataManager.GetRandomName();
            Name = n + " " + fn;
            FirstNameId = fn.Key;
            LastNameId = n.Key;
            Guild = guild;
            Inventory = inventory;
        }

        public TaxCollector(Guild guild,CollectorRecord record)
            : base(record.Id, "",null, new VectorIsometric(Singleton<World>.Instance.GetMap(record.MapId), record.CellId, record.Direction))
        {
            FirstNameId = record.FirstNameId;
            LastNameId = record.LastNameId;
            Name = TaxCollectorDataManager.GetFullName(FirstNameId, LastNameId);
            Guild = guild;
            Inventory = new Inventory(record.Inventory, this);
        }


        public uint FirstNameId
        {
            get;
            set;
        }

        public uint LastNameId
        {
            get;
            set;
        }

        public Guild Guild
        {
            get;
            set;
        }

        public Inventory Inventory
        {
            get;
            set;
        }

        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
           return new GameRolePlayTaxCollectorInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(),FirstNameId,LastNameId,Guild.ToGuildInformations(),Guild.Level);
        }

        public void Trade()
        {
            throw new NotImplementedException();
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }
    }
}