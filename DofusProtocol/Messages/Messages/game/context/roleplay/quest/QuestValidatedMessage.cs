

// Generated on 02/11/2015 10:20:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class QuestValidatedMessage : Message
    {
        public const uint Id = 6097;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short questId;
        
        public QuestValidatedMessage()
        {
        }
        
        public QuestValidatedMessage(short questId)
        {
            this.questId = questId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(questId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questId = reader.ReadVarShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
        }
        
    }
    
}