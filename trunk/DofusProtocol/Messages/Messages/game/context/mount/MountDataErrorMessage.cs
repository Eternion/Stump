
// Generated on 01/04/2013 14:35:46
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountDataErrorMessage : Message
    {
        public const uint Id = 6172;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public MountDataErrorMessage()
        {
        }
        
        public MountDataErrorMessage(sbyte reason)
        {
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reason = reader.ReadSByte();
            if (reason < 0)
                throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
        }
        
    }
    
}