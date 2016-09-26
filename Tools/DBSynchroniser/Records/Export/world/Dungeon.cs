 


// Generated on 09/26/2016 01:50:49
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Dungeons")]
    [D2OClass("Dungeon", "com.ankamagames.dofus.datacenter.world")]
    public class DungeonRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Dungeons";
        public int id;
        [I18NField]
        public uint nameId;
        public int optimalPlayerLevel;
        public List<int> mapIds;
        public int entranceMapId;
        public int exitMapId;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        public int OptimalPlayerLevel
        {
            get { return optimalPlayerLevel; }
            set { optimalPlayerLevel = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> MapIds
        {
            get { return mapIds; }
            set
            {
                mapIds = value;
                m_mapIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_mapIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] MapIdsBin
        {
            get { return m_mapIdsBin; }
            set
            {
                m_mapIdsBin = value;
                mapIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        public int EntranceMapId
        {
            get { return entranceMapId; }
            set { entranceMapId = value; }
        }

        [D2OIgnore]
        public int ExitMapId
        {
            get { return exitMapId; }
            set { exitMapId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Dungeon)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            OptimalPlayerLevel = castedObj.optimalPlayerLevel;
            MapIds = castedObj.mapIds;
            EntranceMapId = castedObj.entranceMapId;
            ExitMapId = castedObj.exitMapId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Dungeon)parent : new Dungeon();
            obj.id = Id;
            obj.nameId = NameId;
            obj.optimalPlayerLevel = OptimalPlayerLevel;
            obj.mapIds = MapIds;
            obj.entranceMapId = EntranceMapId;
            obj.exitMapId = ExitMapId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_mapIdsBin = mapIds == null ? null : mapIds.ToBinary();
        
        }
    }
}