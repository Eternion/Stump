 


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
    [TableName("AlignmentBalance")]
    [D2OClass("AlignmentBalance", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentBalanceRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "AlignmentBalance";
        public int id;
        public int startValue;
        public int endValue;
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
        public int StartValue
        {
            get { return startValue; }
            set { startValue = value; }
        }

        [D2OIgnore]
        public int EndValue
        {
            get { return endValue; }
            set { endValue = value; }
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
            var castedObj = (AlignmentBalance)obj;
            
            Id = castedObj.id;
            StartValue = castedObj.startValue;
            EndValue = castedObj.endValue;
            NameId = castedObj.nameId;
            DescriptionId = castedObj.descriptionId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AlignmentBalance)parent : new AlignmentBalance();
            obj.id = Id;
            obj.startValue = StartValue;
            obj.endValue = EndValue;
            obj.nameId = NameId;
            obj.descriptionId = DescriptionId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}