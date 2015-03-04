

// Generated on 02/19/2015 12:09:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildKickRequestMessage : Message
    {
        public const uint Id = 5887;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int kickedId;
        
        public GuildKickRequestMessage()
        {
        }
        
        public GuildKickRequestMessage(int kickedId)
        {
            this.kickedId = kickedId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(kickedId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            kickedId = reader.ReadVarInt();
            if (kickedId < 0)
                throw new Exception("Forbidden value on kickedId = " + kickedId + ", it doesn't respect the following condition : kickedId < 0");
        }
        
    }
    
}