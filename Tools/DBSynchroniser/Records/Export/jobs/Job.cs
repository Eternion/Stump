 


// Generated on 01/04/2015 01:23:47
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
        public int specializationOfId;
        public int iconId;
        public List<int> toolIds;

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
        public int SpecializationOfId
        {
            get { return specializationOfId; }
            set { specializationOfId = value; }
        }

        [D2OIgnore]
        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> ToolIds
        {
            get { return toolIds; }
            set
            {
                toolIds = value;
                m_toolIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_toolIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ToolIdsBin
        {
            get { return m_toolIdsBin; }
            set
            {
                m_toolIdsBin = value;
                toolIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Job)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            SpecializationOfId = castedObj.specializationOfId;
            IconId = castedObj.iconId;
            ToolIds = castedObj.toolIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (Job)parent : new Job();
            obj.id = Id;
            obj.nameId = NameId;
            obj.specializationOfId = SpecializationOfId;
            obj.iconId = IconId;
            obj.toolIds = ToolIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_toolIdsBin = toolIds == null ? null : toolIds.ToBinary();
        
        }
    }
}