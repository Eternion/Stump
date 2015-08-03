

// Generated on 08/04/2015 00:37:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeKamaModifiedMessage : ExchangeObjectMessage
    {
        public const uint Id = 5521;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int quantity;
        
        public ExchangeKamaModifiedMessage()
        {
        }
        
        public ExchangeKamaModifiedMessage(bool remote, int quantity)
         : base(remote)
        {
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            quantity = reader.ReadVarInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
    }
    
}