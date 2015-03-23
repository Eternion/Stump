using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Maps
{
    public class WorldObjectItem : WorldObject
    {
        public  WorldObjectItem(int id, Map map, Cell cell, ItemTemplate template, int quantity)
        {
            Id = id;
            Position = new ObjectPosition(map, cell);
            Quantity = quantity;
            Item = template;
        }

        public override int Id
        {
            get;
            protected set;
        }

        public ItemTemplate Item
        {
            get;
            protected set;
        }

        public int Quantity
        {
            get;
            protected set;
        }
    }
}
