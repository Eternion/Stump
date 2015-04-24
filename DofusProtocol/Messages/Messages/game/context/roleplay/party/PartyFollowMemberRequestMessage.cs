

// Generated on 04/24/2015 03:38:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyFollowMemberRequestMessage : AbstractPartyMessage
    {
        public const uint Id = 5577;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int playerId;
        
        public PartyFollowMemberRequestMessage()
        {
        }
        
        public PartyFollowMemberRequestMessage(int partyId, int playerId)
         : base(partyId)
        {
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            playerId = reader.ReadVarInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}