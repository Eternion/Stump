using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Plugins.DefaultPlugin.Global.Placements
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

        public bool TestPattern(Map map)
        {
            try
            {
                bool bluesOk;
                bool redsOk;
                if (Relativ)
                {
                    bluesOk = Blues.All(entry => map.GetCell(entry.X + Center.X, entry.Y + Center.Y).Walkable);
                    redsOk = Reds.All(entry => map.GetCell(entry.X + Center.X, entry.Y + Center.Y).Walkable);
                }
                else
                {
                    bluesOk = Blues.All(entry => map.GetCell(entry.X, entry.Y).Walkable);
                    redsOk = Reds.All(entry => map.GetCell(entry.X, entry.Y).Walkable);
                }

                return bluesOk && redsOk;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TestPattern(Point center, Map map)
        {
            try
            {
                bool bluesOk;
                bool redsOk;
                if (Relativ)
                {
                    bluesOk = Blues.All(entry => map.GetCell(entry.X + center.X, entry.Y + center.Y).Walkable);
                    redsOk = Reds.All(entry => map.GetCell(entry.X + center.X, entry.Y + center.Y).Walkable);
                }
                else
                {
                    bluesOk = Blues.All(entry => map.GetCell(entry.X, entry.Y).Walkable);
                    redsOk = Reds.All(entry => map.GetCell(entry.X, entry.Y).Walkable);
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