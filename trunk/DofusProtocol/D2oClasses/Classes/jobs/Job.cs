
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Jobs")]
    [Serializable]
    public class Job : IDataObject, IIndexedData
    {
        private const String MODULE = "Jobs";
        public int id;
        public uint nameId;
        public int specializationOfId;
        public int iconId;
        public List<int> toolIds;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

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

        public List<int> ToolIds
        {
            get { return toolIds; }
            set { toolIds = value; }
        }

    }
}