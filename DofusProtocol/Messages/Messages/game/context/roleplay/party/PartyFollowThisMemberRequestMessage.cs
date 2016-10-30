

// Generated on 10/30/2016 16:20:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyFollowThisMemberRequestMessage : PartyFollowMemberRequestMessage
    {
        public const uint Id = 5588;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool enabled;
        
        public PartyFollowThisMemberRequestMessage()
        {
        }
        
        public PartyFollowThisMemberRequestMessage(int partyId, long playerId, bool enabled)
         : base(partyId, playerId)
        {
            this.enabled = enabled;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(enabled);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            enabled = reader.ReadBoolean();
        }
        
    }
    
}