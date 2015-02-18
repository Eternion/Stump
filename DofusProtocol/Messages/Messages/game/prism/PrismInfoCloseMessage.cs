

// Generated on 02/18/2015 10:46:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismInfoCloseMessage : Message
    {
        public const uint Id = 5853;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PrismInfoCloseMessage()
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