
using ProtoBuf;
using Stump.Core.IO;

namespace Stump.Server.BaseServer.Data.MapTool
{
    [ProtoContract]
    public class CellData
    {

        public CellData()
        {
            
        }
        public CellData(BigEndianReader reader,ushort id,int mapId)
        {
            Id = id;
            MapId=mapId;

            Floor = reader.ReadByte();
            if (Floor * 10 == -1280)
                return;

            LosMov = reader.ReadByte();
            Speed = reader.ReadByte();
            MapChangeData = reader.ReadByte();
        }


        [ProtoMember(1)]
        public ushort Id
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public short Floor
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public byte LosMov
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public byte Speed
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public byte MapChangeData
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public int MapId
        {
            get;
            set;
        }

        public bool Los
        {
            get { return (LosMov & 2) >> 1 == 1; }
        }

        public bool Mov
        {
            get { return (LosMov & 1) == 1 && !NonWalkableDuringFight && !FarmCell; }
        }

        public bool NonWalkableDuringFight
        {
            get { return (LosMov & 4) >> 2 == 1; }
        }

        public bool FarmCell
        {
            get { return (LosMov & 32) >> 5 == 1; }
        }

        public bool Visible
        {
            get { return (LosMov & 64) >> 6 == 1; }
        }

    }
}