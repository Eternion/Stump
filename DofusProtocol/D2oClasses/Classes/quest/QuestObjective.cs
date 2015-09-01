

// Generated on 09/01/2015 11:16:34
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
        public QuestObjectiveParameters parameters;
        public Point coords;
        public int mapId;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint StepId
        {
            get { return this.stepId; }
            set { this.stepId = value; }
        }
        [D2OIgnore]
        public uint TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }
        [D2OIgnore]
        public int DialogId
        {
            get { return this.dialogId; }
            set { this.dialogId = value; }
        }
        [D2OIgnore]
        public QuestObjectiveParameters Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }
        [D2OIgnore]
        public Point Coords
        {
            get { return this.coords; }
            set { this.coords = value; }
        }
        [D2OIgnore]
        public int MapId
        {
            get { return this.mapId; }
            set { this.mapId = value; }
        }
    }
}