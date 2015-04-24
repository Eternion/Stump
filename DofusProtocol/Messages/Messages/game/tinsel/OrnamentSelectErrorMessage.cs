

// Generated on 04/24/2015 03:38:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class OrnamentSelectErrorMessage : Message
    {
        public const uint Id = 6370;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte reason;
        
        public OrnamentSelectErrorMessage()
        {
        }
        
        public OrnamentSelectErrorMessage(sbyte reason)
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