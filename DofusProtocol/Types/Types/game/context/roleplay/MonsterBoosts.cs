

// Generated on 10/30/2016 16:20:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class MonsterBoosts
    {
        public const short Id = 497;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        public short xpBoost;
        public short dropBoost;
        
        public MonsterBoosts()
        {
        }
        
        public MonsterBoosts(int id, short xpBoost, short dropBoost)
        {
            this.id = id;
            this.xpBoost = xpBoost;
            this.dropBoost = dropBoost;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(id);
            writer.WriteVarShort(xpBoost);
            writer.WriteVarShort(dropBoost);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            xpBoost = reader.ReadVarShort();
            if (xpBoost < 0)
                throw new Exception("Forbidden value on xpBoost = " + xpBoost + ", it doesn't respect the following condition : xpBoost < 0");
            dropBoost = reader.ReadVarShort();
            if (dropBoost < 0)
                throw new Exception("Forbidden value on dropBoost = " + dropBoost + ", it doesn't respect the following condition : dropBoost < 0");
        }
        
        
    }
    
}