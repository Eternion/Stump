

// Generated on 04/19/2016 10:17:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class QuestStartRequestMessage : Message
    {
        public const uint Id = 5643;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short questId;
        
        public QuestStartRequestMessage()
        {
        }
        
        public QuestStartRequestMessage(short questId)
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