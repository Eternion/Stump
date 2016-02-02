

// Generated on 02/02/2016 14:14:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ContactLookErrorMessage : Message
    {
        public const uint Id = 6045;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requestId;
        
        public ContactLookErrorMessage()
        {
        }
        
        public ContactLookErrorMessage(int requestId)
        {
            this.requestId = requestId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(requestId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadVarInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
        }
        
    }
    
}