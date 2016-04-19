

// Generated on 04/19/2016 10:17:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InteractiveElementUpdatedMessage : Message
    {
        public const uint Id = 5708;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.InteractiveElement interactiveElement;
        
        public InteractiveElementUpdatedMessage()
        {
        }
        
        public InteractiveElementUpdatedMessage(Types.InteractiveElement interactiveElement)
        {
            this.interactiveElement = interactiveElement;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            interactiveElement.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            interactiveElement = new Types.InteractiveElement();
            interactiveElement.Deserialize(reader);
        }
        
    }
    
}