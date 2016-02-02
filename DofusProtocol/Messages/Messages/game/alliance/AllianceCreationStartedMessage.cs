

// Generated on 02/02/2016 14:14:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceCreationStartedMessage : Message
    {
        public const uint Id = 6394;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceCreationStartedMessage()
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