

// Generated on 04/19/2016 10:17:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectMoveKamaMessage : Message
    {
        public const uint Id = 5520;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int quantity;
        
        public ExchangeObjectMoveKamaMessage()
        {
        }
        
        public ExchangeObjectMoveKamaMessage(int quantity)
        {
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            quantity = reader.ReadVarInt();
        }
        
    }
    
}