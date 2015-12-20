

// Generated on 12/20/2015 16:36:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyRefuseInvitationNotificationMessage : AbstractPartyEventMessage
    {
        public const uint Id = 5596;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long guestId;
        
        public PartyRefuseInvitationNotificationMessage()
        {
        }
        
        public PartyRefuseInvitationNotificationMessage(int partyId, long guestId)
         : base(partyId)
        {
            this.guestId = guestId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(guestId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            guestId = reader.ReadVarLong();
            if (guestId < 0 || guestId > 9.007199254740992E15)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0 || guestId > 9.007199254740992E15");
        }
        
    }
    
}