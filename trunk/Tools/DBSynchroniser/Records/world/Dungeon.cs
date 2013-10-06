 


// Generated on 10/06/2013 18:02:19
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Dungeons")]
    [D2OClass("Dungeon", "com.ankamagames.dofus.datacenter.world")]
    public class DungeonRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Dungeons";
        public int id;
        public uint nameId;
        public int optimalPlayerLevel;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
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

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Dungeon)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            OptimalPlayerLevel = castedObj.optimalPlayerLevel;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Dungeon)parent : new Dungeon();
            obj.id = Id;
            obj.nameId = NameId;
            obj.optimalPlayerLevel = OptimalPlayerLevel;
            return obj;
        
        }
    }
}