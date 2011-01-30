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
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Manager;
using Stump.Server.WorldServer.World.Actors.Actor;

namespace Stump.Server.WorldServer.Entities
{
    public class Merchant : Actor, IInventoryOwner,ITrader
    {

        public Merchant(CharacterRecord record)
            : base(record.Id, record.Name, CharacterManager.GetStuffedCharacterLook(record), new VectorIsometric(World.World.Instance.GetMap(record.MapId), record.CellId, record.Direction))
        {
            Inventory = new Inventory(record.Inventory, this);
            //Guild = record.Guild;
        }

        public Inventory Inventory
        {
            get;
            set;
        }

        public Guild Guild
        {
            get;
            set;
        }

        public override GameRolePlayActorInformations ToGameRolePlayActor()
        {
            if (Guild == null)
                return new GameRolePlayMerchantInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, 0);
            else
                return new GameRolePlayMerchantWithGuildInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, 0, Guild.GetInfo());
        }

    }
}