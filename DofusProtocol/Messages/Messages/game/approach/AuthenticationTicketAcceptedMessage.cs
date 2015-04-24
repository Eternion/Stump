

// Generated on 04/24/2015 03:37:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AuthenticationTicketAcceptedMessage : Message
    {
        public const uint Id = 111;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AuthenticationTicketAcceptedMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}