using System.Drawing;
using Stump.Core.IO;

namespace Stump.DofusProtocol.D2oClasses.Tool.Dlm
{
    public class DlmMap
    {
        public const int CellCount = 560;

        public DlmMap()
        {
            
        }

        public byte Version
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public uint RelativeId
        {
            get;
            set;
        }

        public byte MapType
        {
            get;
            set;
        }

        public int SubAreaId
        {
            get;
            set;
        }

        public int BottomNeighbourId
        {
            get;
            set;
        }

        public int LeftNeighbourId
        {
            get;
            set;
        }

        public int RightNeighbourId
        {
            get;
            set;
        }

        public int ShadowBonusOnEntities
        {
            get;
            set;
        }

        public Color BackgroundColor
        {
            get;
            set;
        }

        public ushort ZoomScale
        {
            get;
            set;
        }

        public short ZoomOffsetX
        {
            get;
            set;
        }

        public short ZoomOffsetY
        {
            get;
            set;
        }

        public bool UseLowPassFilter
        {
            get;
            set;
        }

        public bool UseReverb
        {
            get;
            set;
        }

        public int PresetId
        {
            get;
            set;
        }

        public DlmFixture[] BackgroudFixtures
        {
            get;
            set;
        }

        public int TopNeighbourId
        {
            get;
            set;
        }

        public DlmFixture[] ForegroundFixtures
        {
            get;
            set;
        }

        public DlmCellData[] Cells
        {
            get;
            set;
        }

        public int GroundCRC
        {
            get;
            set;
        }

        public DlmLayer[] Layers
        {
            get;
            set;
        }

        public static DlmMap ReadFromStream(BigEndianReader reader)
        {
            var map = new DlmMap();

            map.Version = reader.ReadByte();
            map.Id = reader.ReadInt();
            map.RelativeId = reader.ReadUInt();
            map.MapType = reader.ReadByte();
            map.SubAreaId = reader.ReadInt();
            map.TopNeighbourId = reader.ReadInt();
            map.BottomNeighbourId = reader.ReadInt();
            map.LeftNeighbourId = reader.ReadInt();
            map.RightNeighbourId = reader.ReadInt();
            map.ShadowBonusOnEntities = reader.ReadInt();

            if (map.Version >= 3)
            {
                map.BackgroundColor = Color.FromArgb(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            }

            if (map.Version >= 4)
            {
                map.ZoomScale = reader.ReadUShort();
                map.ZoomOffsetX = reader.ReadShort();
                map.ZoomOffsetY = reader.ReadShort();
            }

            map.UseLowPassFilter = reader.ReadByte() == 1;
            map.UseReverb = reader.ReadByte() == 1;

            if (map.UseReverb)
            {
                map.PresetId = reader.ReadInt();
            }
            {
                map.PresetId = -1;
            }

            map.BackgroudFixtures = new DlmFixture[reader.ReadByte()];
            for (int i = 0; i < map.BackgroudFixtures.Length; i++)
            {
                map.BackgroudFixtures[i] = DlmFixture.ReadFromStream(map, reader);
            }

            map.ForegroundFixtures = new DlmFixture[reader.ReadByte()];
            for (int i = 0; i < map.ForegroundFixtures.Length; i++)
            {
                map.ForegroundFixtures[i] = DlmFixture.ReadFromStream(map, reader);
            }

            reader.ReadInt();
            map.GroundCRC = reader.ReadInt();

            map.Layers = new DlmLayer[reader.ReadByte()];
            for (int i = 0; i < map.Layers.Length; i++)
            {
                map.Layers[i] = DlmLayer.ReadFromStream(map, reader);
            }

            map.Cells = new DlmCellData[CellCount];
            for (short i = 0; i < map.Cells.Length; i++)
            {
                map.Cells[i] = DlmCellData.ReadFromStream(map, i, reader);
            }

            return map;
        }
    }
}