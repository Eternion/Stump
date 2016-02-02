

// Generated on 02/02/2016 14:14:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class UpdateMountBoost
    {
        public const short Id = 356;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte type;
        
        public UpdateMountBoost()
        {
        }
        
        public UpdateMountBoost(sbyte type)
        {
            this.type = type;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(type);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
        
    }
    
}