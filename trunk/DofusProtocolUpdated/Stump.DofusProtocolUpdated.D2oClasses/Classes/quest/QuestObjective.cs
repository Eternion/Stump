

// Generated on 12/12/2013 16:57:42
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
        public const String MODULE = "QuestObjectives";
        public uint id;
        public uint stepId;
        public uint typeId;
        public int dialogId;
        public List<uint> parameters;
        public Point coords;
        public int mapId;
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
        [D2OIgnore]
        public int MapId
        {
            get { return mapId; }
            set { mapId = value; }
        }
    }
}