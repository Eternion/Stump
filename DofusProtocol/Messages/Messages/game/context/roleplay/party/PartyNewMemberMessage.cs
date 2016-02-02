

// Generated on 02/02/2016 14:14:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyNewMemberMessage : PartyUpdateMessage
    {
        public const uint Id = 6306;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PartyNewMemberMessage()
        {
        }
        
        public PartyNewMemberMessage(int partyId, Types.PartyMemberInformations memberInformations)
         : base(partyId, memberInformations)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}