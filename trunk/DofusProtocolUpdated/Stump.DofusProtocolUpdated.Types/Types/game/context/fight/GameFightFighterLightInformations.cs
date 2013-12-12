

// Generated on 12/12/2013 16:57:30
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
        
        public int id;
        public string name;
        public short level;
        public sbyte breed;
        
        public GameFightFighterLightInformations()
        {
        }
        
        public GameFightFighterLightInformations(int id, string name, short level, sbyte breed)
        {
            this.id = id;
            this.name = name;
            this.level = level;
            this.breed = breed;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteUTF(name);
            writer.WriteShort(level);
            writer.WriteSByte(breed);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            name = reader.ReadUTF();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            breed = reader.ReadSByte();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + Encoding.UTF8.GetByteCount(name) + sizeof(short) + sizeof(sbyte);
        }
        
    }
    
}