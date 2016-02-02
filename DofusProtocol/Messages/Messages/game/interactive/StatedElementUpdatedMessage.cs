

// Generated on 02/02/2016 14:14:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StatedElementUpdatedMessage : Message
    {
        public const uint Id = 5709;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.StatedElement statedElement;
        
        public StatedElementUpdatedMessage()
        {
        }
        
        public StatedElementUpdatedMessage(Types.StatedElement statedElement)
        {
            this.statedElement = statedElement;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            statedElement.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            statedElement = new Types.StatedElement();
            statedElement.Deserialize(reader);
        }
        
    }
    
}