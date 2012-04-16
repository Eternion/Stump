using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;
using Point = System.Drawing.Point;

namespace Stump.Server.WorldServer.Database.World.Maps
{
    [Serializable]
    [ActiveRecord("maps_position")]
    [D2OClass("MapPosition", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapPositionRecord : WorldBaseRecord<MapPositionRecord>
    {
        private Point m_pos;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Foreign, "Id")]
        public int Id
        {
            get;
            set;
        }

        [OneToOne]
        public MapRecord Map
        {
            get;
            set;
        }

        [D2OField("posX")]
        [Property("PosX")]
        public int PosX
        {
            get { return m_pos.X; }
            set { m_pos.X = value; }
        }

        [D2OField("posY")]
        [Property("PosY")]
        public int PosY
        {
            get { return m_pos.Y; }
            set { m_pos.Y = value; }
        }

        public Point Pos
        {
            get { return m_pos; }
            set { m_pos = value; }
        }

        [D2OField("outdoor")]
        [Property("Outdoor")]
        public Boolean Outdoor
        {
            get;
            set;
        }

        [D2OField("subAreaId")]
        [Property("SubAreaId")]
        public int SubAreaId
        {
            get;
            set;
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

        private string m_name;

        public string Name
        {
            get
            {
                return m_name ?? ( m_name = TextManager.Instance.GetText(NameId) );
            }
        }

        [D2OField("hasPriorityOnWorldmap")]
        [Property]
        public bool HasPriorityOnWorldmap
        {
            get;
            set;
        }
    }
}