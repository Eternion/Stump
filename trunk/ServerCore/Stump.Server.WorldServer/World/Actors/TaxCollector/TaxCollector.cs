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
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors.Actor;

namespace Stump.Server.WorldServer.Entities
{
    public class TaxCollector : Actor, IInventoryOwner, ITrader,IAttackable
    {

        protected TaxCollector(Guild guild,long id, ExtendedLook look, VectorIsometric position, Inventory inventory)
            :base(id,"",look,position)
        {
            var fn = TaxCollectorDataManager.GetRandomFirstName();
            var n = TaxCollectorDataManager.GetRandomName();
            Name = n + " " + fn;
            FirstNameId = fn.Key;
            LastNameId = n.Key;
            Guild = guild;
            Inventory = inventory;
        }

        public TaxCollector(Guild guild,CollectorRecord record)
            : base(record.Id, "",null, new VectorIsometric(World.World.Instance.GetMap(record.MapId), record.CellId, record.Direction))
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

    }
}