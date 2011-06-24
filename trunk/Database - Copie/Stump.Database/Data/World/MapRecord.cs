using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.Data.World
{
    [ActiveRecord("maps")]
    public class MapRecord : DataBaseRecord<MapRecord>
    {
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
        public int ZoneId
        {
            get;
            set;
        }

        [BelongsTo("PositionId")]
        public MapPositionRecord Position
        {
            get;
            set;
        }

        public bool Outdoor
        {
            get
            {
                return Position.Outdoor;
            }
            set
            {
                Position.Outdoor = value;
            }
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

        [Property(ColumnType = "Serializable")]
        public List<Cell> Cells
        {
            get;
            set;
        }
    }
}