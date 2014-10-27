

// Generated on 10/27/2014 19:58:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectErrorMessage : Message
    {
        public const uint Id = 3004;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public ObjectErrorMessage()
        {
        }
        
        public ObjectErrorMessage(sbyte reason)
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
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}