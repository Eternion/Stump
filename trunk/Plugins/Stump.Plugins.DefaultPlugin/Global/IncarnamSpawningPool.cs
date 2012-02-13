using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Spawns;

namespace Stump.Plugins.DefaultPlugin.Global
{
    public class IncarnamSpawningPool : ClassicalSpawningPool
    {
        public IncarnamSpawningPool(Map map)
            : base(map)
        {
            GroupSizes = new Dictionary<GroupSize, Tuple<int, int>>
                             {
                                 {GroupSize.Small, Tuple.Create(1, 1)},
                                 {GroupSize.Medium, Tuple.Create(2, 2)},
                                 {GroupSize.Big, Tuple.Create(3, 4)},
                             };
        }

        public IncarnamSpawningPool(Map map, int interval)
            : base(map, interval)
        {
            GroupSizes = new Dictionary<GroupSize, Tuple<int, int>>
                             {
                                 {GroupSize.Small, Tuple.Create(1, 1)},
                                 {GroupSize.Medium, Tuple.Create(2, 2)},
                                 {GroupSize.Big, Tuple.Create(3, 4)},
                             };
        }
    }
}