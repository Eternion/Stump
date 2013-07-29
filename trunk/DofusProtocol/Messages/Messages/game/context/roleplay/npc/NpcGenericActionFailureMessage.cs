

// Generated on 07/29/2013 23:08:00
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NpcGenericActionFailureMessage : Message
    {
        public const uint Id = 5900;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public NpcGenericActionFailureMessage()
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