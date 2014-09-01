

// Generated on 09/01/2014 15:52:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class QuestStepInfoRequestMessage : Message
    {
        public const uint Id = 5622;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public ushort questId;
        
        public QuestStepInfoRequestMessage()
        {
        }
        
        public QuestStepInfoRequestMessage(ushort questId)
        {
            this.questId = questId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(questId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            questId = reader.ReadUShort();
            if (questId < 0 || questId > 65535)
                throw new Exception("Forbidden value on questId = " + questId + ", it doesn't respect the following condition : questId < 0 || questId > 65535");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(ushort);
        }
        
    }
    
}