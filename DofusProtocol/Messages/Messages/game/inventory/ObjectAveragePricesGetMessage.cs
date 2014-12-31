

// Generated on 12/29/2014 21:13:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectAveragePricesGetMessage : Message
    {
        public const uint Id = 6334;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ObjectAveragePricesGetMessage()
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