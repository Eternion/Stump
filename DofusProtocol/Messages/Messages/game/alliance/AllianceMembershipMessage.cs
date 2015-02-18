

// Generated on 02/18/2015 10:46:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceMembershipMessage : AllianceJoinedMessage
    {
        public const uint Id = 6390;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceMembershipMessage()
        {
        }
        
        public AllianceMembershipMessage(Types.AllianceInformations allianceInfo, bool enabled)
         : base(allianceInfo, enabled)
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