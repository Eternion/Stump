

// Generated on 09/26/2016 01:50:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangePlayerMultiCraftRequestMessage : ExchangeRequestMessage
    {
        public const uint Id = 5784;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long target;
        public int skillId;
        
        public ExchangePlayerMultiCraftRequestMessage()
        {
        }
        
        public ExchangePlayerMultiCraftRequestMessage(sbyte exchangeType, long target, int skillId)
         : base(exchangeType)
        {
            this.target = target;
            this.skillId = skillId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(target);
            writer.WriteVarInt(skillId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            target = reader.ReadVarLong();
            if (target < 0 || target > 9007199254740990)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0 || target > 9007199254740990");
            skillId = reader.ReadVarInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
        }
        
    }
    
}