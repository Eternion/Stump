using System;

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

        public int[] Blues
        {
            get;
            set;
        }

        public int[] Reds
        {
            get;
            set;
        }
    }
}