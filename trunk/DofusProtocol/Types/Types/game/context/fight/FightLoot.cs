

// Generated on 07/29/2013 23:08:43
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightLoot
    {
        public const short Id = 41;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> objects;
        public int kamas;
        
        public FightLoot()
        {
        }
        
        public FightLoot(IEnumerable<short> objects, int kamas)
        {
            this.objects = objects;
            this.kamas = kamas;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)objects.Count());
            foreach (var entry in objects)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteInt(kamas);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            objects = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (objects as short[])[i] = reader.ReadShort();
            }
            kamas = reader.ReadInt();
            if (kamas < 0)
                throw new Exception("Forbidden value on kamas = " + kamas + ", it doesn't respect the following condition : kamas < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + objects.Sum(x => sizeof(short)) + sizeof(int);
        }
        
    }
    
}