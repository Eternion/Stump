

// Generated on 08/04/2015 00:35:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ItemDurability
    {
        public const short Id = 168;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short durability;
        public short durabilityMax;
        
        public ItemDurability()
        {
        }
        
        public ItemDurability(short durability, short durabilityMax)
        {
            this.durability = durability;
            this.durabilityMax = durabilityMax;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(durability);
            writer.WriteShort(durabilityMax);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            durability = reader.ReadShort();
            durabilityMax = reader.ReadShort();
        }
        
        
    }
    
}