
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("QuestObjectives")]
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
        public QuestObjectiveType type;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint StepId
        {
            get { return stepId; }
            set { stepId = value; }
        }

        public uint TypeId
        {
            get { return typeId; }
            set { typeId = value; }
        }

        public int DialogId
        {
            get { return dialogId; }
            set { dialogId = value; }
        }

        public List<uint> Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public Point Coords
        {
            get { return coords; }
            set { coords = value; }
        }

        public QuestObjectiveType Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}