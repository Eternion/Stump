

// Generated on 02/11/2015 10:20:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceInsiderInfoRequestMessage : Message
    {
        public const uint Id = 6417;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AllianceInsiderInfoRequestMessage()
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