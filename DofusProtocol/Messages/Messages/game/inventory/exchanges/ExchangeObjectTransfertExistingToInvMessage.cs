

// Generated on 12/20/2015 16:37:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectTransfertExistingToInvMessage : Message
    {
        public const uint Id = 6326;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeObjectTransfertExistingToInvMessage()
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