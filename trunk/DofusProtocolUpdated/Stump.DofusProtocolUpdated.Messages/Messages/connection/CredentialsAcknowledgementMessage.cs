

// Generated on 03/05/2014 20:34:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CredentialsAcknowledgementMessage : Message
    {
        public const uint Id = 6314;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public CredentialsAcknowledgementMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
        public override int GetSerializationSize()
        {
            return 0;
            ;
        }
        
    }
    
}