

// Generated on 08/04/2015 00:37:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyInvitationCancelledForGuestMessage : AbstractPartyMessage
    {
        public const uint Id = 6256;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int cancelerId;
        
        public PartyInvitationCancelledForGuestMessage()
        {
        }
        
        public PartyInvitationCancelledForGuestMessage(int partyId, int cancelerId)
         : base(partyId)
        {
            this.cancelerId = cancelerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(cancelerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cancelerId = reader.ReadVarInt();
            if (cancelerId < 0)
                throw new Exception("Forbidden value on cancelerId = " + cancelerId + ", it doesn't respect the following condition : cancelerId < 0");
        }
        
    }
    
}