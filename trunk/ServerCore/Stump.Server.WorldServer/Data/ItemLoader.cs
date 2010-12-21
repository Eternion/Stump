using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Data.D2oTool;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Data
{
    public static class ItemLoader
    {
        public static void LoadItemNames(ref Dictionary<int, ItemTemplate> itemTemplates)
        {
            foreach (var item in itemTemplates)
            {
                itemTemplates.Where(entry => entry.Value.Id == item.Value.Id)
                    .First().Value.Name = DataLoader.I18NFile.ReadText((int) item.Value.NameId);
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