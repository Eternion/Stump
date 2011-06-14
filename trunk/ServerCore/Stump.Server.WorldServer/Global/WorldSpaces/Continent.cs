
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Database.Data.World;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Global.WorldSpaces;

namespace Stump.Server.WorldServer.Global
{
    public class Continent
    {
        public Continent(SuperAreaRecord record)
        {
            Record = record;
            MapsByPosition = new ConcurrentDictionary<Point, Map>();
        }

        public SuperAreaRecord Record
        {
            get;
            private set;
        }

        public ConcurrentDictionary<Point, Map> MapsByPosition
        {
            get;
            private set;
        }

        public List<Region> Childrens
        {
            get;
            internal set;
        }
    }
}