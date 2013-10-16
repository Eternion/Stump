 


// Generated on 10/13/2013 12:21:16
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
    [TableName("Tips")]
    [D2OClass("Tips", "com.ankamagames.dofus.datacenter.misc")]
    public class TipsRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "Tips";
        public int id;
        public uint descId;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Tips)obj;
            
            Id = castedObj.id;
            DescId = castedObj.descId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (Tips)parent : new Tips();
            obj.id = Id;
            obj.descId = DescId;
            return obj;
        
        }
    }
}