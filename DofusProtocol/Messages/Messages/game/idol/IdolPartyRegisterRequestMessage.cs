

// Generated on 08/04/2015 00:37:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolPartyRegisterRequestMessage : Message
    {
        public const uint Id = 6582;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool register;
        
        public IdolPartyRegisterRequestMessage()
        {
        }
        
        public IdolPartyRegisterRequestMessage(bool register)
        {
            this.register = register;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(register);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            register = reader.ReadBoolean();
        }
        
    }
    
}