 


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
    [D2OClass("QuestObjectives")]
    public class QuestObjectiveRecord : ID2ORecord
    {
        private const String MODULE = "QuestObjectives";
        public uint id;
        public uint stepId;
        public uint typeId;
        public int dialogId;
        public List<uint> parameters;
        public Point coords;
        public QuestObjectiveType type;

        [PrimaryKey("Id", false)]
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
        public byte[] ParametersBin
        {
            get { return m_parametersBin; }
            set
            {
                m_parametersBin = value;
                parameters = value == null ? null : value.ToObject<List<uint>>();
            }
        }

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
        public byte[] CoordsBin
        {
            get { return m_coordsBin; }
            set
            {
                m_coordsBin = value;
                coords = value == null ? null : value.ToObject<Point>();
            }
        }

        [Ignore]
        public QuestObjectiveType Type
        {
            get { return type; }
            set
            {
                type = value;
                m_typeBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_typeBin;
        public byte[] TypeBin
        {
            get { return m_typeBin; }
            set
            {
                m_typeBin = value;
                type = value == null ? null : value.ToObject<QuestObjectiveType>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (QuestObjective)obj;
            
            Id = castedObj.id;
            StepId = castedObj.stepId;
            TypeId = castedObj.typeId;
            DialogId = castedObj.dialogId;
            Parameters = castedObj.parameters;
            Coords = castedObj.coords;
            Type = castedObj.type;
        }
        
        public object CreateObject()
        {
            var obj = new QuestObjective();
            
            obj.id = Id;
            obj.stepId = StepId;
            obj.typeId = TypeId;
            obj.dialogId = DialogId;
            obj.parameters = Parameters;
            obj.coords = Coords;
            obj.type = Type;
            return obj;
        
        }
    }
}