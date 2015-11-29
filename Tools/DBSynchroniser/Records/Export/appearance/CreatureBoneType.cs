 


// Generated on 11/16/2015 14:26:38
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
    [TableName("CreatureBonesTypes")]
    [D2OClass("CreatureBoneType", "com.ankamagames.dofus.datacenter.appearance")]
    public class CreatureBoneTypeRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "CreatureBonesTypes";
        public int id;
        public int creatureBoneId;

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
        public int CreatureBoneId
        {
            get { return creatureBoneId; }
            set { creatureBoneId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (CreatureBoneType)obj;
            
            Id = castedObj.id;
            CreatureBoneId = castedObj.creatureBoneId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (CreatureBoneType)parent : new CreatureBoneType();
            obj.id = Id;
            obj.creatureBoneId = CreatureBoneId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}