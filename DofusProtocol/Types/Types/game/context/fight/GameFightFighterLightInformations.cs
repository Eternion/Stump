

// Generated on 08/04/2015 00:35:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameFightFighterLightInformations
    {
        public const short Id = 413;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public bool sex;
        public bool alive;
        public int id;
        public sbyte wave;
        public short level;
        public sbyte breed;
        
        public GameFightFighterLightInformations()
        {
        }
        
        public GameFightFighterLightInformations(bool sex, bool alive, int id, sbyte wave, short level, sbyte breed)
        {
            this.sex = sex;
            this.alive = alive;
            this.id = id;
            this.wave = wave;
            this.level = level;
            this.breed = breed;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, sex);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, alive);
            writer.WriteByte(flag1);
            writer.WriteInt(id);
            writer.WriteSByte(wave);
            writer.WriteVarShort(level);
            writer.WriteSByte(breed);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            sex = BooleanByteWrapper.GetFlag(flag1, 0);
            alive = BooleanByteWrapper.GetFlag(flag1, 1);
            id = reader.ReadInt();
            wave = reader.ReadSByte();
            if (wave < 0)
                throw new Exception("Forbidden value on wave = " + wave + ", it doesn't respect the following condition : wave < 0");
            level = reader.ReadVarShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            breed = reader.ReadSByte();
        }
        
        
    }
    
}