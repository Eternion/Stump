

// Generated on 08/04/2015 00:36:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceLeftMessage : Message
    {
        public const uint Id = 6398;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceLeftMessage()
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