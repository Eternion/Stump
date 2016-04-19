

// Generated on 04/19/2016 10:17:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismsListUpdateMessage : PrismsListMessage
    {
        public const uint Id = 6438;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PrismsListUpdateMessage()
        {
        }
        
        public PrismsListUpdateMessage(IEnumerable<Types.PrismSubareaEmptyInfo> prisms)
         : base(prisms)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}