

// Generated on 10/26/2014 23:30:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterMinimalInformations : AbstractCharacterInformation
    {
        public const short Id = 110;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public byte level;
        public string name;
        
        public CharacterMinimalInformations()
        {
        }
        
        public CharacterMinimalInformations(int id, byte level, string name)
         : base(id)
        {
            this.level = level;
            this.name = name;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteByte(level);
            writer.WriteUTF(name);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            level = reader.ReadByte();
            if (level < 1 || level > 200)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 1 || level > 200");
            name = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(byte) + sizeof(short) + Encoding.UTF8.GetByteCount(name);
        }
        
    }
    
}