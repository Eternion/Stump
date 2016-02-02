

// Generated on 02/02/2016 14:14:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyLeaderUpdateMessage : AbstractPartyEventMessage
    {
        public const uint Id = 5578;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long partyLeaderId;
        
        public PartyLeaderUpdateMessage()
        {
        }
        
        public PartyLeaderUpdateMessage(int partyId, long partyLeaderId)
         : base(partyId)
        {
            this.partyLeaderId = partyLeaderId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarLong(partyLeaderId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            partyLeaderId = reader.ReadVarLong();
            if (partyLeaderId < 0 || partyLeaderId > 9.007199254740992E15)
                throw new Exception("Forbidden value on partyLeaderId = " + partyLeaderId + ", it doesn't respect the following condition : partyLeaderId < 0 || partyLeaderId > 9.007199254740992E15");
        }
        
    }
    
}