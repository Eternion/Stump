

// Generated on 02/02/2016 14:14:38
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
        
        public int skillId;
        
        public ExchangeStartOkMulticraftCrafterMessage()
        {
        }
        
        public ExchangeStartOkMulticraftCrafterMessage(int skillId)
        {
            this.skillId = skillId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(skillId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            skillId = reader.ReadVarInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
        }
        
    }
    
}