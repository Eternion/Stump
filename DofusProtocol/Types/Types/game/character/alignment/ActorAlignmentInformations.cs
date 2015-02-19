

// Generated on 02/18/2015 11:05:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ActorAlignmentInformations
    {
        public const short Id = 201;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte alignmentSide;
        public sbyte alignmentValue;
        public sbyte alignmentGrade;
        public int characterPower;
        
        public ActorAlignmentInformations()
        {
        }
        
        public ActorAlignmentInformations(sbyte alignmentSide, sbyte alignmentValue, sbyte alignmentGrade, int characterPower)
        {
            this.alignmentSide = alignmentSide;
            this.alignmentValue = alignmentValue;
            this.alignmentGrade = alignmentGrade;
            this.characterPower = characterPower;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(alignmentSide);
            writer.WriteSByte(alignmentValue);
            writer.WriteSByte(alignmentGrade);
            writer.WriteVarInt(characterPower);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            alignmentSide = reader.ReadSByte();
            alignmentValue = reader.ReadSByte();
            if (alignmentValue < 0)
                throw new Exception("Forbidden value on alignmentValue = " + alignmentValue + ", it doesn't respect the following condition : alignmentValue < 0");
            alignmentGrade = reader.ReadSByte();
            if (alignmentGrade < 0)
                throw new Exception("Forbidden value on alignmentGrade = " + alignmentGrade + ", it doesn't respect the following condition : alignmentGrade < 0");
            characterPower = reader.ReadVarInt();
            if (characterPower < 0)
                throw new Exception("Forbidden value on characterPower = " + characterPower + ", it doesn't respect the following condition : characterPower < 0");
        }
        
        
    }
    
}