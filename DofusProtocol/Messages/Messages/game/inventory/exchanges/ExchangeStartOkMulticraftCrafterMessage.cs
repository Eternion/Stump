

// Generated on 04/24/2015 03:38:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkMulticraftCrafterMessage : Message
    {
        public const uint Id = 5818;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte maxCase;
        public int skillId;
        
        public ExchangeStartOkMulticraftCrafterMessage()
        {
        }
        
        public ExchangeStartOkMulticraftCrafterMessage(sbyte maxCase, int skillId)
        {
            this.maxCase = maxCase;
            this.skillId = skillId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(maxCase);
            writer.WriteVarInt(skillId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            maxCase = reader.ReadSByte();
            if (maxCase < 0)
                throw new Exception("Forbidden value on maxCase = " + maxCase + ", it doesn't respect the following condition : maxCase < 0");
            skillId = reader.ReadVarInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
        }
        
    }
    
}