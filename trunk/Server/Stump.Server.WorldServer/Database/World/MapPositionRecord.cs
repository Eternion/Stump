using System;
using System.Collections;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using DPoint = System.Drawing.Point;

namespace Stump.Server.WorldServer.Database.World
{
    [Serializable]
    [ActiveRecord("maps_position")]
    [D2OClass("MapPosition", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapPositionRecord : WorldBaseRecord<MapPositionRecord>
    {
        private DPoint m_pos;
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
            get
            {
                return m_pos.X;
            }
            set
            {
                m_pos.X = value;
            }
        }

        [D2OField("posY")]
        [Property("PosY")]
        public int PosY
        {
            get
            {
                return m_pos.Y;
            }
            set
            {
                m_pos.Y = value;
            }
        }

        public DPoint Pos
        {
            get
            {
                return m_pos;
            }
            set
            {
                m_pos = value;
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
            get
            {
                return m_subArea;
            }
            set
            {
                if (value != null)
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

        protected override bool BeforeSave(IDictionary state)
        {
            if (SubArea == null && SubAreaId > 0)
                SubArea = new SubAreaRecord
                    {
                        Id = SubAreaId
                    };


            return base.BeforeSave(state);
        }
    }
}