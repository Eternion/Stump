using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Maps
{
    public sealed class WorldObjectItem : WorldObject
    {
        public WorldObjectItem(int id, Map map, Cell cell, ItemTemplate template, List<EffectBase> effects, int quantity)
        {
            Id = id;
            Position = new ObjectPosition(map, cell);
            Quantity = quantity;
            Item = template;
            Effects = effects;
            SpawnDate = DateTime.Now;
        }

        public override int Id
        {
            get;
            protected set;
        }

        public ItemTemplate Item
        {
            get;
            private set;
        }

        public List<EffectBase> Effects
        {
            get;
            private set;
        }

        public int Quantity
        {
            get;
            private set;
        }

        public DateTime SpawnDate
        {
            get;
            private set;
        }
    }
}