

// Generated on 08/04/2015 00:37:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyFollowStatusUpdateMessage : AbstractPartyMessage
    {
        public const uint Id = 5581;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool success;
        public int followedId;
        
        public PartyFollowStatusUpdateMessage()
        {
        }
        
        public PartyFollowStatusUpdateMessage(int partyId, bool success, int followedId)
         : base(partyId)
        {
            this.success = success;
            this.followedId = followedId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(success);
            writer.WriteVarInt(followedId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            success = reader.ReadBoolean();
            followedId = reader.ReadVarInt();
            if (followedId < 0)
                throw new Exception("Forbidden value on followedId = " + followedId + ", it doesn't respect the following condition : followedId < 0");
        }
        
    }
    
}