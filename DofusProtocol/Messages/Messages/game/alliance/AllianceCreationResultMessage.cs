

// Generated on 10/30/2016 16:20:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AllianceCreationResultMessage : Message
    {
        public const uint Id = 6391;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte result;
        
        public AllianceCreationResultMessage()
        {
        }
        
        public AllianceCreationResultMessage(sbyte result)
        {
            this.result = result;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(result);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            result = reader.ReadSByte();
            if (result < 0)
                throw new Exception("Forbidden value on result = " + result + ", it doesn't respect the following condition : result < 0");
        }
        
    }
    
}