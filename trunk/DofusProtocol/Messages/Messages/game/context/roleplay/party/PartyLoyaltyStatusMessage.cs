

// Generated on 08/11/2013 11:28:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PartyLoyaltyStatusMessage : AbstractPartyMessage
    {
        public const uint Id = 6270;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool loyal;
        
        public PartyLoyaltyStatusMessage()
        {
        }
        
        public PartyLoyaltyStatusMessage(int partyId, bool loyal)
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
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(bool);
        }
        
    }
    
}