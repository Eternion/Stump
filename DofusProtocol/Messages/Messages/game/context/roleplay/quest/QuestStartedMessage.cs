

// Generated on 11/16/2015 14:26:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class QuestStartedMessage : Message
    {
        public const uint Id = 6091;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short questId;
        
        public QuestStartedMessage()
        {
        }
        
        public QuestStartedMessage(short questId)
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