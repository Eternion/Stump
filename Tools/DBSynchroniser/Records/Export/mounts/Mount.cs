 


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
    [TableName("Mounts")]
    [D2OClass("Mount", "com.ankamagames.dofus.datacenter.mounts")]
    public class MountRecord : ID2ORecord, ISaveIntercepter
    {
        private String MODULE = "Mounts";
        public uint id;
        public uint familyId;
        [I18NField]
        public uint nameId;
        public String look;

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
        public uint FamilyId
        {
            get { return familyId; }
            set { familyId = value; }
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
        public String Look
        {
            get { return look; }
            set { look = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Mount)obj;
            
            Id = castedObj.id;
            FamilyId = castedObj.familyId;
            NameId = castedObj.nameId;
            Look = castedObj.look;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Mount)parent : new Mount();
            obj.id = Id;
            obj.familyId = FamilyId;
            obj.nameId = NameId;
            obj.look = Look;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}