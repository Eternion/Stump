

// Generated on 09/01/2014 15:52:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}