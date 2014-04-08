

// Generated on 03/06/2014 18:50:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeRequestOnMountStockMessage : Message
    {
        public const uint Id = 5986;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeRequestOnMountStockMessage()
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