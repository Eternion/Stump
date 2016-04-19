

// Generated on 04/19/2016 10:17:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyLeaveMessage : AbstractPartyMessage
    {
        public const uint Id = 5594;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PartyLeaveMessage()
        {
        }
        
        public PartyLeaveMessage(int partyId)
         : base(partyId)
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