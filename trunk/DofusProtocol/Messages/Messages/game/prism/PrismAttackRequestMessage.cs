

// Generated on 07/26/2013 22:51:07
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismAttackRequestMessage : Message
    {
        public const uint Id = 6042;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PrismAttackRequestMessage()
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