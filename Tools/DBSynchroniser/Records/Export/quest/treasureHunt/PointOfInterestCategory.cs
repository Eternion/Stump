 


// Generated on 01/04/2015 01:23:48
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
    [TableName("PointOfInterestCategory")]
    [D2OClass("PointOfInterestCategory", "com.ankamagames.dofus.datacenter.quest.treasureHunt")]
    public class PointOfInterestCategoryRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "PointOfInterestCategory";
        public uint id;
        [I18NField]
        public uint actionLabelId;

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
        public uint ActionLabelId
        {
            get { return actionLabelId; }
            set { actionLabelId = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (PointOfInterestCategory)obj;
            
            Id = castedObj.id;
            ActionLabelId = castedObj.actionLabelId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (PointOfInterestCategory)parent : new PointOfInterestCategory();
            obj.id = Id;
            obj.actionLabelId = ActionLabelId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}