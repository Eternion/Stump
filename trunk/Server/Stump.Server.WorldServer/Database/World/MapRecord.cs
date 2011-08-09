using System;
using System.Collections;
using Castle.ActiveRecord;
using Stump.Core.IO;

namespace Stump.Server.WorldServer.Database.World
{
    [ActiveRecord("maps")]
    public class MapRecord : WorldBaseRecord<MapRecord>
    {
        private byte[] m_compressedCells;

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
            get { return Position != null ? Position.Outdoor : false; }
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

        public Cell[] Cells
        {
            get;
            set;
        }

        protected override bool BeforeSave(IDictionary state)
        {
            CompressedCells = new byte[Cells.Length*Cell.StructSize];

            for (int i = 0; i < Cells.Length; i++)
            {
                Array.Copy(Cells[i].Serialize(), 0, CompressedCells, i*Cell.StructSize, Cell.StructSize);
            }

            CompressedCells = ZipHelper.Compress(CompressedCells);

            return base.BeforeSave(state);
        }
    }
}