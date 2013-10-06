 


// Generated on 10/06/2013 18:02:18
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("QuestObjectives")]
    [D2OClass("QuestObjective", "com.ankamagames.dofus.datacenter.quest")]
    public class QuestObjectiveRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "QuestObjectives";
        public uint id;
        public uint stepId;
        public uint typeId;
        public int dialogId;
        public List<uint> parameters;
        public Point coords;

        [D2OIgnore]
        [PrimaryKey("Id", false)]
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
        [Ignore]
        public List<uint> Parameters
        {
            get { return parameters; }
            set
            {
                parameters = value;
                m_parametersBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_parametersBin;
        [D2OIgnore]
        public byte[] ParametersBin
        {
            get { return m_parametersBin; }
            set
            {
                m_parametersBin = value;
                parameters = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public Point Coords
        {
            get { return coords; }
            set
            {
                coords = value;
                m_coordsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_coordsBin;
        [D2OIgnore]
        public byte[] CoordsBin
        {
            get { return m_coordsBin; }
            set
            {
                m_coordsBin = value;
                coords = value == null ? null : value.ToObject<Point>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (QuestObjective)obj;
            
            Id = castedObj.id;
            StepId = castedObj.stepId;
            TypeId = castedObj.typeId;
            DialogId = castedObj.dialogId;
            Parameters = castedObj.parameters;
            Coords = castedObj.coords;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (QuestObjective)parent : new QuestObjective();
            obj.id = Id;
            obj.stepId = StepId;
            obj.typeId = TypeId;
            obj.dialogId = DialogId;
            obj.parameters = Parameters;
            obj.coords = Coords;
            return obj;
        
        }
    }
}