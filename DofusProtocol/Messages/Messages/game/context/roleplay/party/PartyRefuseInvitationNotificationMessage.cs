

// Generated on 04/19/2016 10:17:25
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
            if (guestId < 0 || guestId > 9007199254740990)
                throw new Exception("Forbidden value on guestId = " + guestId + ", it doesn't respect the following condition : guestId < 0 || guestId > 9007199254740990");
        }
        
    }
    
}