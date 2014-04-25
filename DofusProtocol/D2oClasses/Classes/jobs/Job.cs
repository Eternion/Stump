

// Generated on 10/28/2013 14:03:19
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Job", "com.ankamagames.dofus.datacenter.jobs")]
    [Serializable]
    public class Job : IDataObject, IIndexedData
    {
        private const String MODULE = "Jobs";
        public int id;
        [I18NField]
        public uint nameId;
        public int specializationOfId;
        public int iconId;
        public List<int> toolIds;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
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
        public List<int> ToolIds
        {
            get { return toolIds; }
            set { toolIds = value; }
        }
    }
}