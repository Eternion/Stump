 


// Generated on 09/26/2016 01:50:46
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
    [TableName("MountFamily")]
    [D2OClass("MountFamily", "com.ankamagames.dofus.datacenter.mounts")]
    public class MountFamilyRecord : ID2ORecord, ISaveIntercepter
    {
        private String MODULE = "MountFamily";
        public uint id;
        [I18NField]
        public uint nameId;
        public String headUri;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
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
        [NullString]
        public String HeadUri
        {
            get { return headUri; }
            set { headUri = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MountFamily)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            HeadUri = castedObj.headUri;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (MountFamily)parent : new MountFamily();
            obj.id = Id;
            obj.nameId = NameId;
            obj.headUri = HeadUri;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}