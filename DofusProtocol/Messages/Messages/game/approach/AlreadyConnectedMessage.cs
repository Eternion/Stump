

// Generated on 01/04/2015 11:54:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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