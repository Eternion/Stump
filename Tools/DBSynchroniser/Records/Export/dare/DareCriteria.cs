 


// Generated on 09/26/2016 01:50:41
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
    [TableName("DareCriterias")]
    [D2OClass("DareCriteria", "com.ankamagames.dofus.datacenter.dare")]
    public class DareCriteriaRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "DareCriterias";
        public uint id;
        [I18NField]
        public uint nameId;
        public uint maxOccurence;
        public uint maxParameters;
        public int minParameterBound;
        public int maxParameterBound;

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
        public uint MaxOccurence
        {
            get { return maxOccurence; }
            set { maxOccurence = value; }
        }

        [D2OIgnore]
        public uint MaxParameters
        {
            get { return maxParameters; }
            set { maxParameters = value; }
        }

        [D2OIgnore]
        public int MinParameterBound
        {
            get { return minParameterBound; }
            set { minParameterBound = value; }
        }

        [D2OIgnore]
        public int MaxParameterBound
        {
            get { return maxParameterBound; }
            set { maxParameterBound = value; }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (DareCriteria)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            MaxOccurence = castedObj.maxOccurence;
            MaxParameters = castedObj.maxParameters;
            MinParameterBound = castedObj.minParameterBound;
            MaxParameterBound = castedObj.maxParameterBound;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (DareCriteria)parent : new DareCriteria();
            obj.id = Id;
            obj.nameId = NameId;
            obj.maxOccurence = MaxOccurence;
            obj.maxParameters = MaxParameters;
            obj.minParameterBound = MinParameterBound;
            obj.maxParameterBound = MaxParameterBound;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
        
        }
    }
}