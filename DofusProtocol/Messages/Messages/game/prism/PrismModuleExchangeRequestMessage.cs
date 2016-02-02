

// Generated on 02/02/2016 14:14:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismModuleExchangeRequestMessage : Message
    {
        public const uint Id = 6531;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PrismModuleExchangeRequestMessage()
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