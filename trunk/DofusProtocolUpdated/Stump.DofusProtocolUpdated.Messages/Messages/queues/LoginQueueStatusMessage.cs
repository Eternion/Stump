

// Generated on 03/06/2014 18:50:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class LoginQueueStatusMessage : Message
    {
        public const uint Id = 10;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public ushort position;
        public ushort total;
        
        public LoginQueueStatusMessage()
        {
        }
        
        public LoginQueueStatusMessage(ushort position, ushort total)
        {
            this.position = position;
            this.total = total;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(position);
            writer.WriteUShort(total);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            position = reader.ReadUShort();
            if (position < 0 || position > 65535)
                throw new Exception("Forbidden value on position = " + position + ", it doesn't respect the following condition : position < 0 || position > 65535");
            total = reader.ReadUShort();
            if (total < 0 || total > 65535)
                throw new Exception("Forbidden value on total = " + total + ", it doesn't respect the following condition : total < 0 || total > 65535");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(ushort) + sizeof(ushort);
        }
        
    }
    
}