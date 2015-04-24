

// Generated on 04/24/2015 03:37:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SequenceStartMessage : Message
    {
        public const uint Id = 955;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte sequenceType;
        public int authorId;
        
        public SequenceStartMessage()
        {
        }
        
        public SequenceStartMessage(sbyte sequenceType, int authorId)
        {
            this.sequenceType = sequenceType;
            this.authorId = authorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(sequenceType);
            writer.WriteInt(authorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sequenceType = reader.ReadSByte();
            authorId = reader.ReadInt();
        }
        
    }
    
}