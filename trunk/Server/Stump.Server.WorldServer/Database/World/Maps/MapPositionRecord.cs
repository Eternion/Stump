using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Castle.ActiveRecord;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Point = System.Drawing.Point;

namespace Stump.Server.WorldServer.Database.Maps
{
    public class MapPositionRecordConfiguration : EntityTypeConfiguration<MapPositionRecord>
    {
        public MapPositionRecordConfiguration()
        {
            ToTable("maps_positions");
            
        }
    }
    [D2OClass("MapPosition", "com.ankamagames.dofus.datacenter.world")]
    public sealed class MapPositionRecord : IAssignedByD2O, ISaveIntercepter
    {
        private Point m_pos;

        public int Id
        {
            get;
            set;
        }

        public MapRecord Map
        {
            get;
            set;
        }

        public int PosX
        {
            get { return m_pos.X; }
            set { m_pos.X = value; }
        }

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

        public Boolean Outdoor
        {
            get;
            set;
        }

        public int SubAreaId
        {
            get;
            set;
        }

        public int Capabilities
        {
            get;
            set;
        }

        public int WorldMap
        {
            get;
            set;
        }

        private byte[] m_soundsBin;

        public byte[] SoundsBin
        {
            get { return m_soundsBin; }
            set { m_soundsBin = value;
            Sounds = value.ToObject<List<AmbientSound>>();
            }
        }

        public List<AmbientSound> Sounds
        {
            get;
            set;
        }

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

        public bool HasPriorityOnWorldmap
        {
            get;
            set;
        }

        public void AssignFields(object d2oObject)
        {
            var map = (DofusProtocol.D2oClasses.MapPosition)d2oObject;
            Id = map.id;
            NameId = map.nameId;
            PosX = map.posX;
            PosY = map.posY;
            Outdoor = map.outdoor;
            SubAreaId = map.subAreaId;
            Capabilities = map.capabilities;
            WorldMap = map.worldMap;
            Sounds = map.sounds;
            HasPriorityOnWorldmap = map.hasPriorityOnWorldmap;
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            m_soundsBin = Sounds.ToBinary();
        }
    }
}