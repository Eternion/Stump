

// Generated on 02/19/2015 12:09:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidSearchOkMessage : Message
    {
        public const uint Id = 5802;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeBidSearchOkMessage()
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