

// Generated on 02/11/2015 10:20:37
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
        
        public int humanVendorId;
        public short humanVendorCell;
        
        public ExchangeOnHumanVendorRequestMessage()
        {
        }
        
        public ExchangeOnHumanVendorRequestMessage(int humanVendorId, short humanVendorCell)
        {
            this.humanVendorId = humanVendorId;
            this.humanVendorCell = humanVendorCell;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(humanVendorId);
            writer.WriteVarShort(humanVendorCell);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            humanVendorId = reader.ReadVarInt();
            if (humanVendorId < 0)
                throw new Exception("Forbidden value on humanVendorId = " + humanVendorId + ", it doesn't respect the following condition : humanVendorId < 0");
            humanVendorCell = reader.ReadVarShort();
            if (humanVendorCell < 0 || humanVendorCell > 559)
                throw new Exception("Forbidden value on humanVendorCell = " + humanVendorCell + ", it doesn't respect the following condition : humanVendorCell < 0 || humanVendorCell > 559");
        }
        
    }
    
}