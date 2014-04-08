

// Generated on 03/02/2014 20:42:45
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
        
        public ushort questId;
        
        public QuestStartRequestMessage()
        {
        }
        
        public QuestStartRequestMessage(ushort questId)
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