

// Generated on 10/30/2016 16:20:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicAckMessage : Message
    {
        public const uint Id = 6362;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int seq;
        public short lastPacketId;
        
        public BasicAckMessage()
        {
        }
        
        public BasicAckMessage(int seq, short lastPacketId)
        {
            this.seq = seq;
            this.lastPacketId = lastPacketId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(seq);
            writer.WriteVarShort(lastPacketId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            seq = reader.ReadVarInt();
            if (seq < 0)
                throw new Exception("Forbidden value on seq = " + seq + ", it doesn't respect the following condition : seq < 0");
            lastPacketId = reader.ReadVarShort();
            if (lastPacketId < 0)
                throw new Exception("Forbidden value on lastPacketId = " + lastPacketId + ", it doesn't respect the following condition : lastPacketId < 0");
        }
        
    }
    
}