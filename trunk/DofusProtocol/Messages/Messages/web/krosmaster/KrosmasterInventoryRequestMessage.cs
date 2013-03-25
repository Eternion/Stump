
// Generated on 03/25/2013 19:24:26
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class KrosmasterInventoryRequestMessage : Message
    {
        public const uint Id = 6344;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public KrosmasterInventoryRequestMessage()
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