using System;

namespace Stump.Server.WorldServer.Database.World
{
    // this struct have to be light because of the instance number (560 * 10000 = 53MB) 
    [Serializable]
    public struct Cell
    {
        public short Id;
        public short Floor;
        public byte LosMov;
        public byte MapChangeData;
        public byte Speed;
    }
}