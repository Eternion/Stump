

// Generated on 12/20/2015 16:37:08
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismUseRequestMessage : Message
    {
        public const uint Id = 6041;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte moduleToUse;
        
        public PrismUseRequestMessage()
        {
        }
        
        public PrismUseRequestMessage(sbyte moduleToUse)
        {
            this.moduleToUse = moduleToUse;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(moduleToUse);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            moduleToUse = reader.ReadSByte();
            if (moduleToUse < 0)
                throw new Exception("Forbidden value on moduleToUse = " + moduleToUse + ", it doesn't respect the following condition : moduleToUse < 0");
        }
        
    }
    
}