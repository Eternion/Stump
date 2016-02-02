 


// Generated on 02/02/2016 14:15:18
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
    [TableName("PointOfInterest")]
    [D2OClass("PointOfInterest", "com.ankamagames.dofus.datacenter.quest.treasureHunt")]
    public class PointOfInterestRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "PointOfInterest";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint categoryId;

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
        public uint CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (PointOfInterest)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            CategoryId = castedObj.categoryId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (PointOfInterest)parent : new PointOfInterest();
            obj.id = Id;
            obj.nameId = NameId;
            obj.categoryId = CategoryId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}