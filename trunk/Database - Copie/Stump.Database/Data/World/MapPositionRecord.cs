using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.DofusProtocol.D2oClasses.Classes.ambientSounds;
using Stump.DofusProtocol.D2oClasses.Tool;
using DPoint = System.Drawing.Point;

namespace Stump.Database.Data.World
{
    [Serializable]
    [ActiveRecord("map_position")]
    [AttributeAssociatedFile("MapPositions")]
    [D2OClass("MapPosition", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapPositionRecord : DataBaseRecord<MapPositionRecord>
    {
        private int m_posX;
        private int m_posY;
        private SubAreaRecord m_subArea;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("posX")]
        [Property("PosX")]
        public int PosX
        {
            get { return m_posX; }
            set
            {
                m_posX = value;
                Pos = new DPoint(PosX, PosY);
            }
        }

        [D2OField("posY")]
        [Property("PosY")]
        public int PosY
        {
            get { return m_posY; }
            set
            {
                m_posY = value;
                Pos = new DPoint(PosX, PosY);
            }
        }

        private DPoint m_pos;

        public DPoint Pos
        {
            get { return m_pos; }
            set
            {
                m_pos = value;
                PosX = value.X;
                PosY = value.Y;
            }
        }

        [D2OField("outdoor")]
        [Property("Outdoor")]
        public Boolean Outdoor
        {
            get;
            set;
        }

        /// <summary>
        /// Internal Only. Do not use
        /// </summary>
        [D2OField("subAreaId")]
        public int SubAreaId
        {
            get;
            set;
        }

        [BelongsTo("SubAreaId")]
        public SubAreaRecord SubArea
        {
            get { return m_subArea; }
            set
            {
                SubAreaId = value.Id;
                m_subArea = value;
            }
        }

        [D2OField("capabilities")]
        [Property("Capabilities")]
        public int Capabilities
        {
            get;
            set;
        }

        [D2OField("worldMap")]
        [Property("WorldMap")]
        public int WorldMap
        {
            get;
            set;
        }

        [D2OField("sounds")]
        [Property("Sounds", ColumnType = "Serializable")]
        public List<AmbientSound> Sounds
        {
            get;
            set;
        }

        [D2OField("nameId")]
        [Property("NameId")]
        public int NameId
        {
            get;
            set;
        }
    }
}