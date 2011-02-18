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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Data.D2oTool;
using Stump.Server.DataProvider.Data;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Data
{
    public static class ItemLoader
    {
        public static void LoadItemNames(ref Dictionary<int, ItemTemplate> itemTemplates)
        {
            foreach (var item in itemTemplates)
            {
                item.Value.Name = D2OLoader.I18NFile.ReadText((int) item.Value.NameId);
            }

        }

        public static void LoadItemsStored(ref Dictionary<int, ItemTemplate> itemTemplates)
        {
            // todo
            /*var items = ItemTemplates;
            Parallel.ForEach(Directory.GetFiles(Definitions.CONTENT_PATH + "items/", "*.item"), (string file) =>
            {
                Tesla.WorldServer.Items.ItemStored item = Utilities.UnserializeBytesToObject<Tesla.WorldServer.Items.ItemStored>(File.ReadAllBytes(file),
                    new Tesla.WorldServer.Items.ItemStoredBinder());

                // Startup crash fix. Null check
                if (item != null)
                {
                    if (items.ContainsKey(item.Id))
                        items[item.Id].ItemStored = item;
                }
            });
            ItemTemplates = items;*/
        }
    }
}