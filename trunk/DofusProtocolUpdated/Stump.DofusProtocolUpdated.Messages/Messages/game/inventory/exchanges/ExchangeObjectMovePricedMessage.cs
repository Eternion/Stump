

// Generated on 03/05/2014 20:34:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectMovePricedMessage : ExchangeObjectMoveMessage
    {
        public const uint Id = 5514;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int price;
        
        public ExchangeObjectMovePricedMessage()
        {
        }
        
        public ExchangeObjectMovePricedMessage(int objectUID, int quantity, int price)
         : base(objectUID, quantity)
        {
            this.price = price;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(price);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            price = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}