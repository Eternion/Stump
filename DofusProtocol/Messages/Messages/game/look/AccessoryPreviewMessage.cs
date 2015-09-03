

// Generated on 09/01/2015 10:48:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AccessoryPreviewMessage : Message
    {
        public const uint Id = 6517;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.EntityLook look;
        
        public AccessoryPreviewMessage()
        {
        }
        
        public AccessoryPreviewMessage(Types.EntityLook look)
        {
            this.look = look;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            look.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            look = new Types.EntityLook();
            look.Deserialize(reader);
        }
        
    }
    
}