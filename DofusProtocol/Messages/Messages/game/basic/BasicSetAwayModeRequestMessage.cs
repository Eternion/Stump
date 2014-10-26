

// Generated on 10/26/2014 23:29:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicSetAwayModeRequestMessage : Message
    {
        public const uint Id = 5665;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public BasicSetAwayModeRequestMessage()
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