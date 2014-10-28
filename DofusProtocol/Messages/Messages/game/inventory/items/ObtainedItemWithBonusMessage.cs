

// Generated on 10/28/2014 16:37:01
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
        
        public ObtainedItemWithBonusMessage(int genericId, int baseQuantity, int bonusQuantity)
         : base(genericId, baseQuantity)
        {
            this.bonusQuantity = bonusQuantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(bonusQuantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            bonusQuantity = reader.ReadInt();
            if (bonusQuantity < 0)
                throw new Exception("Forbidden value on bonusQuantity = " + bonusQuantity + ", it doesn't respect the following condition : bonusQuantity < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}