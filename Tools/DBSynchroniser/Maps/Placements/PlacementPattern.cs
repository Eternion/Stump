using System;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using DBSynchroniser.Records.Maps;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace DBSynchroniser.Maps.Placements
{
    [Serializable]
    public class PlacementPattern
    {
        public bool Relativ
        {
            get;
            set;
        }

        public Point[] Blues
        {
            get;
            set;
        }

        public Point[] Reds
        {
            get;
            set;
        }

        public Point Center
        {
            get;
            set;
        }

        [XmlIgnore]
        public int Complexity
        {
            get;
            set;
        }

        public bool TestPattern(MapRecord map)
        {
            try
            {
                bool bluesOk;
                bool redsOk;
                if (Relativ)
                {
                    bluesOk = Blues.All(entry => GetCell(map, entry.X + Center.X, entry.Y + Center.Y).Walkable);
                    redsOk = Reds.All(entry => GetCell(map, entry.X + Center.X, entry.Y + Center.Y).Walkable);
                }
                else
                {
                    bluesOk = Blues.All(entry => GetCell(map, entry.X, entry.Y).Walkable);
                    redsOk = Reds.All(entry => GetCell(map, entry.X, entry.Y).Walkable);
                }

                return bluesOk && redsOk;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Cell GetCell(MapRecord map, int x, int y)
        {
            var point = new MapPoint(x, y);
            return map.Cells[point.CellId];
        }

       public bool TestPattern(Point center, MapRecord map)
        {
            try
            {
                bool bluesOk;
                bool redsOk;
                if (Relativ)
                {
                    bluesOk = Blues.All(entry => GetCell(map, entry.X + center.X, entry.Y + center.Y).Walkable);
                    redsOk = Reds.All(entry => GetCell(map, entry.X + center.X, entry.Y + center.Y).Walkable);
                }
                else
                {
                    bluesOk = Blues.All(entry => GetCell(map, entry.X, entry.Y).Walkable);
                    redsOk = Reds.All(entry => GetCell(map, entry.X, entry.Y).Walkable);
                }

                return bluesOk && redsOk;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}