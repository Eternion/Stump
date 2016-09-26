

// Generated on 09/26/2016 01:50:03
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
        
        public long cancelerId;
        
        public PartyInvitationCancelledForGuestMessage()
        {
        }
        
        public PartyInvitationCancelledForGuestMessage(int partyId, long cancelerId)
         : base(partyId)
        {
            this.cancelerId = cancelerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(cancelerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cancelerId = reader.ReadVarLong();
            if (cancelerId < 0 || cancelerId > 9007199254740990)
                throw new Exception("Forbidden value on cancelerId = " + cancelerId + ", it doesn't respect the following condition : cancelerId < 0 || cancelerId > 9007199254740990");
        }
        
    }
    
}