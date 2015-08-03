

// Generated on 08/04/2015 00:37:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkMulticraftCustomerMessage : Message
    {
        public const uint Id = 5817;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int skillId;
        public byte crafterJobLevel;
        
        public ExchangeStartOkMulticraftCustomerMessage()
        {
        }
        
        public ExchangeStartOkMulticraftCustomerMessage(int skillId, byte crafterJobLevel)
        {
            this.skillId = skillId;
            this.crafterJobLevel = crafterJobLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(skillId);
            writer.WriteByte(crafterJobLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            skillId = reader.ReadVarInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            crafterJobLevel = reader.ReadByte();
            if (crafterJobLevel < 0 || crafterJobLevel > 255)
                throw new Exception("Forbidden value on crafterJobLevel = " + crafterJobLevel + ", it doesn't respect the following condition : crafterJobLevel < 0 || crafterJobLevel > 255");
        }
        
    }
    
}