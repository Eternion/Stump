
// Generated on 03/25/2013 19:24:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestObjectiveTypes")]
    [Serializable]
    public class QuestObjectiveType : IDataObject, IIndexedData
    {
        private const String MODULE = "QuestObjectiveTypes";
        public uint id;
        public uint nameId;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

    }
}