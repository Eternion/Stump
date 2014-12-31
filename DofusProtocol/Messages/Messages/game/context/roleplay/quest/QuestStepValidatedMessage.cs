

// Generated on 12/29/2014 21:12:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class QuestStepValidatedMessage : Message
    {
        public const uint Id = 6099;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short questId;
        public short stepId;
        
        public QuestStepValidatedMessage()
        {
        }
        
        public QuestStepValidatedMessage(short questId, short stepId)
        {
            this.questId = questId;
            this.stepId = stepId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(questId);
            writer.WriteShort(stepId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questId = reader.ReadShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
            stepId = reader.ReadShort();
            if (stepId < 0)
                throw new Exception("Forbidden value on stepId = " + stepId + ", it doesn't respect the following condition : stepId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short);
        }
        
    }
    
}