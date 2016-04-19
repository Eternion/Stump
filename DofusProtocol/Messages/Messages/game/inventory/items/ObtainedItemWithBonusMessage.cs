

// Generated on 04/19/2016 10:17:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObtainedItemWithBonusMessage : ObtainedItemMessage
    {
        public const uint Id = 6520;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int bonusQuantity;
        
        public ObtainedItemWithBonusMessage()
        {
        }
        
        public ObtainedItemWithBonusMessage(short genericId, int baseQuantity, int bonusQuantity)
         : base(genericId, baseQuantity)
        {
            this.bonusQuantity = bonusQuantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(bonusQuantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            bonusQuantity = reader.ReadVarInt();
            if (bonusQuantity < 0)
                throw new Exception("Forbidden value on bonusQuantity = " + bonusQuantity + ", it doesn't respect the following condition : bonusQuantity < 0");
        }
        
    }
    
}