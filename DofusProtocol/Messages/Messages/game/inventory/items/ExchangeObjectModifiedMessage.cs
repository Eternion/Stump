

// Generated on 08/04/2015 13:25:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectModifiedMessage : ExchangeObjectMessage
    {
        public const uint Id = 5519;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem @object;
        
        public ExchangeObjectModifiedMessage()
        {
        }
        
        public ExchangeObjectModifiedMessage(bool remote, Types.ObjectItem @object)
         : base(remote)
        {
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            @object.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            @object = new Types.ObjectItem();
            @object.Deserialize(reader);
        }
        
    }
    
}