

// Generated on 09/26/2016 01:50:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterHardcoreOrEpicInformations : CharacterBaseInformations
    {
        public const short Id = 474;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte deathState;
        public short deathCount;
        public byte deathMaxLevel;
        
        public CharacterHardcoreOrEpicInformations()
        {
        }
        
        public CharacterHardcoreOrEpicInformations(long id, string name, byte level, Types.EntityLook entityLook, sbyte breed, bool sex, sbyte deathState, short deathCount, byte deathMaxLevel)
         : base(id, name, level, entityLook, breed, sex)
        {
            this.deathState = deathState;
            this.deathCount = deathCount;
            this.deathMaxLevel = deathMaxLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(deathState);
            writer.WriteVarShort(deathCount);
            writer.WriteByte(deathMaxLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            deathState = reader.ReadSByte();
            if (deathState < 0)
                throw new Exception("Forbidden value on deathState = " + deathState + ", it doesn't respect the following condition : deathState < 0");
            deathCount = reader.ReadVarShort();
            if (deathCount < 0)
                throw new Exception("Forbidden value on deathCount = " + deathCount + ", it doesn't respect the following condition : deathCount < 0");
            deathMaxLevel = reader.ReadByte();
            if (deathMaxLevel < 1 || deathMaxLevel > 200)
                throw new Exception("Forbidden value on deathMaxLevel = " + deathMaxLevel + ", it doesn't respect the following condition : deathMaxLevel < 1 || deathMaxLevel > 200");
        }
        
        
    }
    
}