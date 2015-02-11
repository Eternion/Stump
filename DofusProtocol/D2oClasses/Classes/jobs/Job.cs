

// Generated on 02/11/2015 10:21:32
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
        public const String MODULE = "Jobs";
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return this.nameId; }
            set { this.nameId = value; }
        }
        [D2OIgnore]
        public int SpecializationOfId
        {
            get { return this.specializationOfId; }
            set { this.specializationOfId = value; }
        }
        [D2OIgnore]
        public int IconId
        {
            get { return this.iconId; }
            set { this.iconId = value; }
        }
        [D2OIgnore]
        public List<int> ToolIds
        {
            get { return this.toolIds; }
            set { this.toolIds = value; }
        }
    }
}