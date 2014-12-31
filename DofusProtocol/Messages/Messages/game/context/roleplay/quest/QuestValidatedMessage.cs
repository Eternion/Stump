

// Generated on 12/29/2014 21:12:59
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
            writer.WriteShort(questId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questId = reader.ReadShort();
            if (questId < 0)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}