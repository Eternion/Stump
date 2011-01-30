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
using System.Linq;
using Stump.BaseCore.Framework.Extensions;
using Stump.Database;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Effects;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Items
{

    public class Storage : IInventoryOwner
    {

        public Storage(StorageRecord record)
        {
            Record =record;
            Id = record.StorageId;
            Password = record.Password;
            Inventory = new Inventory(record.Inventory, this);
        }

        public readonly StorageRecord Record;

        public readonly uint Id;

        public string Password
        {
            get;
            set;
        }

        public readonly Inventory Inventory;


        public void Save()
        {
            Record.Password = Password;
            Inventory.Save();
            Record.SaveAndFlush();
        }

    }
}