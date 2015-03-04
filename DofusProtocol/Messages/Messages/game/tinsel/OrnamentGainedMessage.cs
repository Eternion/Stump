

// Generated on 02/19/2015 12:09:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class OrnamentGainedMessage : Message
    {
        public const uint Id = 6368;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short ornamentId;
        
        public OrnamentGainedMessage()
        {
        }
        
        public OrnamentGainedMessage(short ornamentId)
        {
            this.ornamentId = ornamentId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(ornamentId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            ornamentId = reader.ReadShort();
            if (ornamentId < 0)
                throw new Exception("Forbidden value on ornamentId = " + ornamentId + ", it doesn't respect the following condition : ornamentId < 0");
        }
        
    }
    
}