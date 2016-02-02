

// Generated on 02/02/2016 14:14:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChangeHavenBagRoomRequestMessage : Message
    {
        public const uint Id = 6638;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte roomId;
        
        public ChangeHavenBagRoomRequestMessage()
        {
        }
        
        public ChangeHavenBagRoomRequestMessage(sbyte roomId)
        {
            this.roomId = roomId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(roomId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            roomId = reader.ReadSByte();
            if (roomId < 0)
                throw new Exception("Forbidden value on roomId = " + roomId + ", it doesn't respect the following condition : roomId < 0");
        }
        
    }
    
}