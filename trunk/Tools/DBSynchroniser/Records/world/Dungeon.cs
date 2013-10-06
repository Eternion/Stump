 


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
    [TableName("Dungeons")]
    [D2OClass("Dungeon")]
    public class DungeonRecord : ID2ORecord
    {
        private const String MODULE = "Dungeons";
        public int id;
        public uint nameId;
        public int optimalPlayerLevel;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new Dungeon();
            obj.id = Id;
            obj.nameId = NameId;
            obj.optimalPlayerLevel = OptimalPlayerLevel;
            return obj;
        
        }
    }
}