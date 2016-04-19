

// Generated on 04/19/2016 10:17:10
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
        public double authorId;
        
        public SequenceStartMessage()
        {
        }
        
        public SequenceStartMessage(sbyte sequenceType, double authorId)
        {
            this.sequenceType = sequenceType;
            this.authorId = authorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(sequenceType);
            writer.WriteDouble(authorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sequenceType = reader.ReadSByte();
            authorId = reader.ReadDouble();
            if (authorId < -9007199254740990 || authorId > 9007199254740990)
                throw new Exception("Forbidden value on authorId = " + authorId + ", it doesn't respect the following condition : authorId < -9007199254740990 || authorId > 9007199254740990");
        }
        
    }
    
}