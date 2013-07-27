

// Generated on 07/26/2013 22:51:03
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseItemRemoveOkMessage : Message
    {
        public const uint Id = 5946;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int sellerId;
        
        public ExchangeBidHouseItemRemoveOkMessage()
        {
        }
        
        public ExchangeBidHouseItemRemoveOkMessage(int sellerId)
        {
            this.sellerId = sellerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(sellerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sellerId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}