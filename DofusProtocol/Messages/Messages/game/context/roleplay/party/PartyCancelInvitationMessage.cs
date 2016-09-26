

// Generated on 09/26/2016 01:50:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyCancelInvitationMessage : AbstractPartyMessage
    {
        public const uint Id = 6254;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long guestId;
        
        public PartyCancelInvitationMessage()
        {
        }
        
        public PartyCancelInvitationMessage(int partyId, long guestId)
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