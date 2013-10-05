 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Jobs")]
    public class JobRecord : ID2ORecord
    {
        private const String MODULE = "Jobs";
        public int id;
        public uint nameId;
        public int specializationOfId;
        public int iconId;
        public List<int> toolIds;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int SpecializationOfId
        {
            get { return specializationOfId; }
            set { specializationOfId = value; }
        }

        public int IconId
        {
            get { return iconId; }
            set { iconId = value; }
        }

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
        public byte[] ToolIdsBin
        {
            get { return m_toolIdsBin; }
            set
            {
                m_toolIdsBin = value;
                toolIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (Job)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            SpecializationOfId = castedObj.specializationOfId;
            IconId = castedObj.iconId;
            ToolIds = castedObj.toolIds;
        }
        
        public object CreateObject()
        {
            var obj = new Job();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.specializationOfId = SpecializationOfId;
            obj.iconId = IconId;
            obj.toolIds = ToolIds;
            return obj;
        
        }
    }
}