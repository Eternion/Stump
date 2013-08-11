

// Generated on 08/11/2013 11:29:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ContactLookRequestMessage : Message
    {
        public const uint Id = 5932;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public byte requestId;
        public sbyte contactType;
        
        public ContactLookRequestMessage()
        {
        }
        
        public ContactLookRequestMessage(byte requestId, sbyte contactType)
        {
            this.requestId = requestId;
            this.contactType = contactType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte(requestId);
            writer.WriteSByte(contactType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadByte();
            if (requestId < 0 || requestId > 255)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0 || requestId > 255");
            contactType = reader.ReadSByte();
            if (contactType < 0)
                throw new Exception("Forbidden value on contactType = " + contactType + ", it doesn't respect the following condition : contactType < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(byte) + sizeof(sbyte);
        }
        
    }
    
}