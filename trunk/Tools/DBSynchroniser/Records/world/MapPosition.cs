 


// Generated on 10/06/2013 14:22:02
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("MapPositions")]
    [D2OClass("MapPosition")]
    public class MapPositionRecord : ID2ORecord
    {
        private const String MODULE = "MapPositions";
        public int id;
        public int posX;
        public int posY;
        public Boolean outdoor;
        public int capabilities;
        public int nameId;
        public List<AmbientSound> sounds;
        public int subAreaId;
        public int worldMap;
        public Boolean hasPriorityOnWorldmap;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int PosX
        {
            get { return posX; }
            set { posX = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public Boolean Outdoor
        {
            get { return outdoor; }
            set { outdoor = value; }
        }

        public int Capabilities
        {
            get { return capabilities; }
            set { capabilities = value; }
        }

        public int NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [Ignore]
        public List<AmbientSound> Sounds
        {
            get { return sounds; }
            set
            {
                sounds = value;
                m_soundsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_soundsBin;
        public byte[] SoundsBin
        {
            get { return m_soundsBin; }
            set
            {
                m_soundsBin = value;
                sounds = value == null ? null : value.ToObject<List<AmbientSound>>();
            }
        }

        public int SubAreaId
        {
            get { return subAreaId; }
            set { subAreaId = value; }
        }

        public int WorldMap
        {
            get { return worldMap; }
            set { worldMap = value; }
        }

        public Boolean HasPriorityOnWorldmap
        {
            get { return hasPriorityOnWorldmap; }
            set { hasPriorityOnWorldmap = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MapPosition)obj;
            
            Id = castedObj.id;
            PosX = castedObj.posX;
            PosY = castedObj.posY;
            Outdoor = castedObj.outdoor;
            Capabilities = castedObj.capabilities;
            NameId = castedObj.nameId;
            Sounds = castedObj.sounds;
            SubAreaId = castedObj.subAreaId;
            WorldMap = castedObj.worldMap;
            HasPriorityOnWorldmap = castedObj.hasPriorityOnWorldmap;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new MapPosition();
            obj.id = Id;
            obj.posX = PosX;
            obj.posY = PosY;
            obj.outdoor = Outdoor;
            obj.capabilities = Capabilities;
            obj.nameId = NameId;
            obj.sounds = Sounds;
            obj.subAreaId = SubAreaId;
            obj.worldMap = WorldMap;
            obj.hasPriorityOnWorldmap = HasPriorityOnWorldmap;
            return obj;
        
        }
    }
}