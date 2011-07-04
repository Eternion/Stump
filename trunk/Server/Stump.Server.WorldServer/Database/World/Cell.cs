namespace Stump.Server.WorldServer.Database.World
{
    public struct Cell
    {
        public ushort Id;
        public short Floor;
        public byte LosMov;
        public byte MapChangeData;
        public byte Speed;
    }
}