

// Generated on 10/30/2016 16:20:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyCancelInvitationNotificationMessage : AbstractPartyEventMessage
    {
        public const uint Id = 6251;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long cancelerId;
        public long guestId;
        
        public PartyCancelInvitationNotificationMessage()
        {
        }
        
        public PartyCancelInvitationNotificationMessage(int partyId, long cancelerId, long guestId)
         : base(partyId)
        {
            this.cancelerId = cancelerId;
            this.guestId = guestId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(cancelerId);
            writer.WriteVarLong(guestId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            cancelerId = reader.ReadVarLong();
            if (cancelerId < 0 || cancelerId > 9007199254740990)
                throw new Exception("Forbidden value on cancelerId = " + cancelerId + ", it doesn't respect the following condition : cancelerId < 0 || cancelerId > 9007199254740990");
            guestId = reader.ReadVarLong();
            if (guestId < 0 || guestId > 9007199254740990)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0 || guestId > 9007199254740990");
        }
        
    }
    
}