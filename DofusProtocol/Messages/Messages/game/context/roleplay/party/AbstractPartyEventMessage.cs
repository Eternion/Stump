

// Generated on 10/30/2016 16:20:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AbstractPartyEventMessage : AbstractPartyMessage
    {
        public const uint Id = 6273;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AbstractPartyEventMessage()
        {
        }
        
        public AbstractPartyEventMessage(int partyId)
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