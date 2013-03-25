
// Generated on 03/25/2013 19:24:18
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorErrorMessage : Message
    {
        public const uint Id = 5634;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public TaxCollectorErrorMessage()
        {
        }
        
        public TaxCollectorErrorMessage(sbyte reason)
        {
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reason = reader.ReadSByte();
        }
        
    }
    
}