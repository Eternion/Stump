

// Generated on 09/26/2016 01:49:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SequenceEndMessage : Message
    {
        public const uint Id = 956;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short actionId;
        public double authorId;
        public sbyte sequenceType;
        
        public SequenceEndMessage()
        {
        }
        
        public SequenceEndMessage(short actionId, double authorId, sbyte sequenceType)
        {
            this.actionId = actionId;
            this.authorId = authorId;
            this.sequenceType = sequenceType;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(actionId);
            writer.WriteDouble(authorId);
            writer.WriteSByte(sequenceType);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actionId = reader.ReadVarShort();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            authorId = reader.ReadDouble();
            if (authorId < -9007199254740990 || authorId > 9007199254740990)
                throw new Exception("Forbidden value on authorId = " + authorId + ", it doesn't respect the following condition : authorId < -9007199254740990 || authorId > 9007199254740990");
            sequenceType = reader.ReadSByte();
        }
        
    }
    
}