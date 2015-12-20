 


// Generated on 12/20/2015 18:16:37
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
    [TableName("Challenge")]
    [D2OClass("Challenge", "com.ankamagames.dofus.datacenter.challenges")]
    public class ChallengeRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "Challenge";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descriptionId;

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
        [I18NField]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Challenge)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Challenge)parent : new Challenge();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}