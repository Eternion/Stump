

// Generated on 03/05/2014 20:34:47
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
        public short level;
        public sbyte breed;
        
        public GameFightFighterLightInformations()
        {
        }
        
        public GameFightFighterLightInformations(int id, short level, sbyte breed)
        {
            this.id = id;
            this.level = level;
            this.breed = breed;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteShort(level);
            writer.WriteSByte(breed);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            level = reader.ReadShort();
            if (level < 0)
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : level < 0");
            breed = reader.ReadSByte();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + sizeof(sbyte);
        }
        
    }
    
}