

// Generated on 09/01/2014 15:51:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SequenceNumberMessage : Message
    {
        public const uint Id = 6317;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public ushort number;
        
        public SequenceNumberMessage()
        {
        }
        
        public SequenceNumberMessage(ushort number)
        {
            this.number = number;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort(number);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            number = reader.ReadUShort();
            if (number < 0 || number > 65535)
                throw new Exception("Forbidden value on number = " + number + ", it doesn't respect the following condition : number < 0 || number > 65535");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(ushort);
        }
        
    }
    
}