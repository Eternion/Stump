

// Generated on 08/04/2015 13:25:05
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class QuestStepStartedMessage : Message
    {
        public const uint Id = 6096;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short questId;
        public short stepId;
        
        public QuestStepStartedMessage()
        {
        }
        
        public QuestStepStartedMessage(short questId, short stepId)
        {
            this.questId = questId;
            this.stepId = stepId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(questId);
            writer.WriteVarShort(stepId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questId = reader.ReadVarShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
            stepId = reader.ReadVarShort();
            if (stepId < 0)
                throw new Exception("Forbidden value on stepId = " + stepId + ", it doesn't respect the following condition : stepId < 0");
        }
        
    }
    
}