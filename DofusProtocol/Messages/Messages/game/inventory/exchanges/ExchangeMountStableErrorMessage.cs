

// Generated on 04/19/2016 10:17:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeMountStableErrorMessage : Message
    {
        public const uint Id = 5981;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeMountStableErrorMessage()
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