

// Generated on 09/26/2016 01:50:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeOnHumanVendorRequestMessage : Message
    {
        public const uint Id = 5772;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long humanVendorId;
        public short humanVendorCell;
        
        public ExchangeOnHumanVendorRequestMessage()
        {
        }
        
        public ExchangeOnHumanVendorRequestMessage(long humanVendorId, short humanVendorCell)
        {
            this.humanVendorId = humanVendorId;
            this.humanVendorCell = humanVendorCell;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(humanVendorId);
            writer.WriteVarShort(humanVendorCell);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            humanVendorId = reader.ReadVarLong();
            if (humanVendorId < 0 || humanVendorId > 9007199254740990)
                throw new Exception("Forbidden value on humanVendorId = " + humanVendorId + ", it doesn't respect the following condition : humanVendorId < 0 || humanVendorId > 9007199254740990");
            humanVendorCell = reader.ReadVarShort();
            if (humanVendorCell < 0 || humanVendorCell > 559)
                throw new Exception("Forbidden value on humanVendorCell = " + humanVendorCell + ", it doesn't respect the following condition : humanVendorCell < 0 || humanVendorCell > 559");
        }
        
    }
    
}