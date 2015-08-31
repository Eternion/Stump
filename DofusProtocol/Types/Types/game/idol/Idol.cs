

// Generated on 08/04/2015 00:35:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class Idol
    {
        public const short Id = 489;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short id;
        public short xpBonusPercent;
        public short dropBonusPercent;
        
        public Idol()
        {
        }
        
        public Idol(short id, short xpBonusPercent, short dropBonusPercent)
        {
            this.id = id;
            this.xpBonusPercent = xpBonusPercent;
            this.dropBonusPercent = dropBonusPercent;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(id);
            writer.WriteVarShort(xpBonusPercent);
            writer.WriteVarShort(dropBonusPercent);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            xpBonusPercent = reader.ReadVarShort();
            if (xpBonusPercent < 0)
                throw new Exception("Forbidden value on xpBonusPercent = " + xpBonusPercent + ", it doesn't respect the following condition : xpBonusPercent < 0");
            dropBonusPercent = reader.ReadVarShort();
            if (dropBonusPercent < 0)
                throw new Exception("Forbidden value on dropBonusPercent = " + dropBonusPercent + ", it doesn't respect the following condition : dropBonusPercent < 0");
        }
        
        
    }
    
}