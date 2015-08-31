 


// Generated on 08/13/2015 17:50:45
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
    [TableName("Jobs")]
    [D2OClass("Job", "com.ankamagames.dofus.datacenter.jobs")]
    public class JobRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Jobs";
        public int id;
        [I18NField]
        public uint nameId;
        public int iconId;

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
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Job)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            IconId = castedObj.iconId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Job)parent : new Job();
            obj.id = Id;
            obj.nameId = NameId;
            obj.iconId = IconId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}