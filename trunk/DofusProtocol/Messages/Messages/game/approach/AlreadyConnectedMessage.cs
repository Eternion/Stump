
// Generated on 03/25/2013 19:24:01
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AlreadyConnectedMessage : Message
    {
        public const uint Id = 109;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AlreadyConnectedMessage()
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