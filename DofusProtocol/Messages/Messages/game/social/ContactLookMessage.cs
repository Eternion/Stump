

// Generated on 04/19/2016 10:17:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ContactLookMessage : Message
    {
        public const uint Id = 5934;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requestId;
        public string playerName;
        public long playerId;
        public Types.EntityLook look;
        
        public ContactLookMessage()
        {
        }
        
        public ContactLookMessage(int requestId, string playerName, long playerId, Types.EntityLook look)
        {
            this.requestId = requestId;
            this.playerName = playerName;
            this.playerId = playerId;
            this.look = look;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(requestId);
            writer.WriteUTF(playerName);
            writer.WriteVarLong(playerId);
            look.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadVarInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            playerName = reader.ReadUTF();
            playerId = reader.ReadVarLong();
            if (playerId < 0 || playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            look = new Types.EntityLook();
            look.Deserialize(reader);
        }
        
    }
    
}