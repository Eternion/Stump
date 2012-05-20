using System;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace ArkalysPlugin.Commands
{
    [Serializable]
    public class Isle
    {
        public Isle()
        {
            
        }

        public Isle(byte level, int startMap, short startCell, byte startDirection)
        {
            Level = level;
            StartMap = startMap;
            StartDirection = startDirection;
            StartCell = startCell;
        }

        public byte Level
        {
            get;
            set;
        }

        public int StartMap
        {
            get;
            set;
        }

        public short StartCell
        {
            get;
            set;
        }

        public byte StartDirection
        {
            get;
            set;
        }

        public double KamasRate
        {
            get;
            set;
        }

        public double XPRate
        {
            get;
            set;
        }

        public double StatsModifier
        {
            get;
            set;
        }

        public int[] SubAreas
        {
            get;
            set;
        }

        public bool CanJoinIsle(Character character)
        {
            return character.Level >= Level;
        }
    }
}