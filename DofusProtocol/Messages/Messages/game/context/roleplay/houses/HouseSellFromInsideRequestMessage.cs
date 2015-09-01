

// Generated on 09/01/2015 10:48:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseSellFromInsideRequestMessage : HouseSellRequestMessage
    {
        public const uint Id = 5884;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public HouseSellFromInsideRequestMessage()
        {
        }
        
        public HouseSellFromInsideRequestMessage(int amount)
         : base(amount)
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