

// Generated on 10/28/2013 14:03:20
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestObjective", "com.ankamagames.dofus.datacenter.quest")]
    [Serializable]
    public class QuestObjective : IDataObject, IIndexedData
    {
        private const String MODULE = "QuestObjectives";
        public uint id;
        public uint stepId;
        public uint typeId;
        public int dialogId;
        public List<uint> parameters;
        public Point coords;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint StepId
        {
            get { return stepId; }
            set { stepId = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }
        [D2OIgnore]
        public int DialogId
        {
            get { return dialogId; }
            set { dialogId = value; }
        }
        [D2OIgnore]
        public List<uint> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }
        [D2OIgnore]
        public Point Coords
        {
            get { return coords; }
            set { coords = value; }
        }
    }
}