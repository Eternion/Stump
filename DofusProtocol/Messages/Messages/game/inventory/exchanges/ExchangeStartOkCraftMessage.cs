

// Generated on 09/26/2016 01:50:13
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkCraftMessage : Message
    {
        public const uint Id = 5813;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeStartOkCraftMessage()
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