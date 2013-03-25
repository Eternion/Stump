
// Generated on 03/25/2013 19:24:20
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeRequestOnTaxCollectorMessage : Message
    {
        public const uint Id = 5779;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int taxCollectorId;
        
        public ExchangeRequestOnTaxCollectorMessage()
        {
        }
        
        public ExchangeRequestOnTaxCollectorMessage(int taxCollectorId)
        {
            this.taxCollectorId = taxCollectorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(taxCollectorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            taxCollectorId = reader.ReadInt();
        }
        
    }
    
}