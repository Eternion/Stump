

// Generated on 08/04/2015 13:25:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyPledgeLoyaltyRequestMessage : AbstractPartyMessage
    {
        public const uint Id = 6269;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool loyal;
        
        public PartyPledgeLoyaltyRequestMessage()
        {
        }
        
        public PartyPledgeLoyaltyRequestMessage(int partyId, bool loyal)
         : base(partyId)
        {
            this.loyal = loyal;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(loyal);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            loyal = reader.ReadBoolean();
        }
        
    }
    
}