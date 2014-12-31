

// Generated on 12/29/2014 21:13:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeShopStockMovementUpdatedMessage : Message
    {
        public const uint Id = 5909;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItemToSell objectInfo;
        
        public ExchangeShopStockMovementUpdatedMessage()
        {
        }
        
        public ExchangeShopStockMovementUpdatedMessage(Types.ObjectItemToSell objectInfo)
        {
            this.objectInfo = objectInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            objectInfo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectInfo = new Types.ObjectItemToSell();
            objectInfo.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return objectInfo.GetSerializationSize();
        }
        
    }
    
}