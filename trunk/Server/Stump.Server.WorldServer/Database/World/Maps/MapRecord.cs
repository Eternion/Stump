using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Core.IO;

namespace Stump.Server.WorldServer.Database.World.Maps
{
    [ActiveRecord("maps")]
    public class MapRecord : WorldBaseRecord<MapRecord>
    {
        private byte[] m_compressedCells;
        private byte[] m_compressedElements;

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        ///   Map version of this map.
        /// </summary>
        [Property]
        public uint Version
        {
            get;
            set;
        }

        /// <summary>
        ///   Relative id of this map.
        /// </summary>.
        [Property]
        public uint RelativeId
        {
            get;
            set;
        }

        /// <summary>
        ///   Type of this map.
        /// </summary>
        [Property]
        public int MapType
        {
            get;
            set;
        }

        /// <summary>
        ///   Zone Id which owns this map.
        /// </summary>
        [Property]
        public int SubAreaId
        {
            get;
            set;
        }

        [OneToOne]
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

        [Property]
        public int TopNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int BottomNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int LeftNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int RightNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int ClientTopNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int ClientBottomNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int ClientLeftNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int ClientRightNeighbourId
        {
            get;
            set;
        }

        [Property]
        public int ShadowBonusOnEntities
        {
            get;
            set;
        }

        [Property]
        public bool UseLowpassFilter
        {
            get;
            set;
        }

        [Property]
        public bool UseReverb
        {
            get;
            set;
        }

        [Property]
        public int PresetId
        {
            get;
            set;
        }

        private short[] m_blueCells;
        private short[] m_redCells;

        [Field("BlueCells")]
        private byte[] m_rawBlueCells;

        [Field("RedCells")]
        private byte[] m_rawRedCells;

        public short[] BlueFightCells
        {
            get
            {
                return m_rawBlueCells == null ? new short[0] : (m_blueCells ?? ( m_blueCells = DeserializeFightCells(m_rawBlueCells) ));
            }
            set
            {
                m_blueCells = value;

                m_rawBlueCells = value != null ? SerializeFightCells(value) : null;
            }
        }

        public short[] RedFightCells
        {
            get
            {
                return m_rawRedCells == null ? new short[0] : (m_redCells ?? ( m_redCells = DeserializeFightCells(m_rawRedCells) ));
            }
            set
            {
                m_redCells = value;
                m_rawRedCells = value != null ? SerializeFightCells(value) : null;
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

        [Property(ColumnType = "BinaryBlob", NotNull = true)]
        private byte[] CompressedCells
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

        [Property(ColumnType = "BinaryBlob", NotNull = true)]
        private byte[] CompressedElements
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

        protected override bool OnFlushDirty(object id, IDictionary previousState, IDictionary currentState, NHibernate.Type.IType[] types)
        {
            m_compressedCells = new byte[Cells.Length * Cell.StructSize];

            for (int i = 0; i < Cells.Length; i++)
            {
                Array.Copy(Cells[i].Serialize(), 0, m_compressedCells, i * Cell.StructSize, Cell.StructSize);
            }

            m_compressedCells = (byte[])(currentState["CompressedCells"] = ZipHelper.Compress(m_compressedCells));

            m_compressedElements = new byte[Elements.Length * MapElement.Size];
            for (int i = 0; i < Elements.Length; i++)
            {
                Array.Copy(Elements[i].Serialize(), 0, m_compressedElements, i * MapElement.Size, MapElement.Size);
            }

            m_compressedElements = (byte[])(currentState["CompressedElements"] = ZipHelper.Compress(m_compressedElements));

            return base.OnFlushDirty(id, previousState, currentState, types);
        }

        protected override bool BeforeSave(IDictionary state)
        {
            m_compressedCells = new byte[Cells.Length * Cell.StructSize];

            for (int i = 0; i < Cells.Length; i++)
            {
                Array.Copy(Cells[i].Serialize(), 0, m_compressedCells, i * Cell.StructSize, Cell.StructSize);
            }

            m_compressedCells = (byte[])( state["CompressedCells"] = ZipHelper.Compress(m_compressedCells) );

            m_compressedElements = new byte[Elements.Length * MapElement.Size];
            for (int i = 0; i < Elements.Length; i++)
            {
                Array.Copy(Elements[i].Serialize(), 0, m_compressedElements, i * MapElement.Size, MapElement.Size);
            }

            m_compressedElements = (byte[])( state["CompressedElements"] = ZipHelper.Compress(m_compressedElements) );

            return base.BeforeSave(state);
        }

    }
}