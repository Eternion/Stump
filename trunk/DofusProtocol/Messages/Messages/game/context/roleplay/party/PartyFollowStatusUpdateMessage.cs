
// Generated on 03/25/2013 19:24:13
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteInt(followedId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            success = reader.ReadBoolean();
            followedId = reader.ReadInt();
            if (followedId < 0)
                throw new Exception("Forbidden value on followedId = " + followedId + ", it doesn't respect the following condition : followedId < 0");
        }
        
    }
    
}