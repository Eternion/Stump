

// Generated on 04/19/2016 10:17:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ServerSessionConstant
    {
        public const short Id = 430;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short id;
        
        public ServerSessionConstant()
        {
        }
        
        public ServerSessionConstant(short id)
        {
            this.id = id;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(id);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
        }
        
        
    }
    
}