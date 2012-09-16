using System;
using System.Collections;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Core.IO;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Database.Maps
{
    public class MapRecordConfiguration : EntityTypeConfiguration<MapRecord>
    {
        public MapRecordConfiguration()
        {
            ToTable("maps");
            HasRequired(x => x.Position);
            Ignore(x => x.Outdoor);
        }
    } 
    public class MapRecord : ISaveIntercepter
    {
        private byte[] m_compressedCells;
        private byte[] m_compressedElements;

        public int Id
        {
            get;
            set;
        }

        /// <summary>
        ///   Map version of this map.
        /// </summary>
        public uint Version
        {
            get;
            set;
        }

        /// <summary>
        ///   Relative id of this map.
        /// </summary>
        public uint RelativeId
        {
            get;
            set;
        }

        /// <summary>
        ///   Type of this map.
        /// </summary>
        public int MapType
        {
            get;
            set;
        }

        /// <summary>
        ///   Zone Id which owns this map.
        /// </summary>
        public int SubAreaId
        {
            get;
            set;
        }

        public MapPositionRecord Position
        {
            get;
            set;
        }

        public bool Outdoor
        {
            get { return Position != null && Position.Outdoor; }
            set { if (Position != null) Position.Outdoor = value; }
        }

        public int TopNeighbourId
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

        public int ClientTopNeighbourId
        {
            get;
            set;
        }

        public int ClientBottomNeighbourId
        {
            get;
            set;
        }

        public int ClientLeftNeighbourId
        {
            get;
            set;
        }

        public int ClientRightNeighbourId
        {
            get;
            set;
        }

        public int ShadowBonusOnEntities
        {
            get;
            set;
        }

        public bool UseLowpassFilter
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

        private short[] m_blueCells;
        private short[] m_redCells;

        public byte[] BlueCellsBin
        {
            get;
            set;
        }

        public byte[] RedCellsBin
        {
            get;
            set;
        }

        public short[] BlueFightCells
        {
            get
            {
                return BlueCellsBin == null ? new short[0] : ( m_blueCells ?? ( m_blueCells = DeserializeFightCells(BlueCellsBin) ) );
            }
            set
            {
                m_blueCells = value;

                BlueCellsBin = value != null ? SerializeFightCells(value) : null;
            }
        }

        public short[] RedFightCells
        {
            get
            {
                return RedCellsBin == null ? new short[0] : ( m_redCells ?? ( m_redCells = DeserializeFightCells(RedCellsBin) ) );
            }
            set
            {
                m_redCells = value;
                RedCellsBin = value != null ? SerializeFightCells(value) : null;
            }
        }

        public static byte[] SerializeFightCells(short[] cells)
        {
            var bytes = new byte[cells.Length * 2];

            for (int i = 0, l = 0; i < cells.Length; i++, l += 2)
            {
                bytes[l] = (byte)( ( cells[i] & 0xFF00 ) >> 8 );
                bytes[l + 1] = (byte)( cells[i] & 0xFF );
            }

            return bytes;
        }

        public static short[] DeserializeFightCells(byte[] bytes)
        {
            if (( bytes.Length % 2 ) != 0)
                throw new ArgumentException("bytes.Length % 2 != 0");

            var cells = new short[bytes.Length / 2];

            for (int i = 0, j = 0; i < bytes.Length; i += 2, j++)
                cells[j] = (short)( bytes[i] << 8 | bytes[i + 1] );

            return cells;
        }

        public byte[] CompressedCells
        {
            get { return m_compressedCells; }
            set
            {
                m_compressedCells = value;
                byte[] uncompressedCells = ZipHelper.Uncompress(m_compressedCells);

                Cells = new Cell[uncompressedCells.Length/Cell.StructSize];
                for (int i = 0, j = 0; i < uncompressedCells.Length; i += Cell.StructSize, j++)
                {
                    Cells[j] = new Cell();
                    Cells[j].Deserialize(uncompressedCells, i);
                }
            }
        }

        public byte[] CompressedElements
        {
            get { return m_compressedElements; }
            set
            {
                m_compressedElements = value;
                byte[] uncompressedElements = ZipHelper.Uncompress(m_compressedElements);

                Elements = new MapElement[uncompressedElements.Length / MapElement.Size];
                for (int i = 0, j = 0; i < uncompressedElements.Length; i += MapElement.Size, j++)
                {
                    var element = new MapElement();
                    element.Deserialize(uncompressedElements, i);

                    Elements[j] = element;
                }
            }
        }

        public MapElement[] FindMapElement(int id)
        {
            return Elements.Where(entry => entry.ElementId == id).ToArray();
        }

        public MapElement[] Elements
        {
            get;
            set;
        }

        public Cell[] Cells
        {
            get;
            set;
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_compressedCells = new byte[Cells.Length * Cell.StructSize];

            for (int i = 0; i < Cells.Length; i++)
            {
                Array.Copy(Cells[i].Serialize(), 0, m_compressedCells, i * Cell.StructSize, Cell.StructSize);
            }

            m_compressedCells = ZipHelper.Compress(m_compressedCells);

            m_compressedElements = new byte[Elements.Length * MapElement.Size];
            for (int i = 0; i < Elements.Length; i++)
            {
                Array.Copy(Elements[i].Serialize(), 0, m_compressedElements, i * MapElement.Size, MapElement.Size);
            }

            m_compressedElements = ZipHelper.Compress(m_compressedElements);
        }
    }
}