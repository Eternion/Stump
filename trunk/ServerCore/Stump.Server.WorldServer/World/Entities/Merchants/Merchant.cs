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
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Helpers;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Guilds;
using Stump.Server.WorldServer.World.Storages;

namespace Stump.Server.WorldServer.World.Entities.Merchants
{
    public class Merchant : Actor, IInventoryOwner,ITrader
    {

        public Merchant(CharacterRecord record)
            : base(record.Id, record.Name, CharacterManager.GetStuffedCharacterLook(record), new VectorIsometric(Singleton<World>.Instance.GetMap(record.MapId), record.CellId, record.Direction))
        {
            m_record = record;
            Inventory = new Inventory(this, m_record.Inventory);
            //Guild = record.Guild;
        }

        private readonly CharacterRecord m_record;

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

        public void Trade()
        {
            throw new NotImplementedException();
        }
    }
}