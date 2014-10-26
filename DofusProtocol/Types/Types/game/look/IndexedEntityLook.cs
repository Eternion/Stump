

// Generated on 10/26/2014 23:30:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class IndexedEntityLook
    {
        public const short Id = 405;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public Types.EntityLook look;
        public sbyte index;
        
        public IndexedEntityLook()
        {
        }
        
        public IndexedEntityLook(Types.EntityLook look, sbyte index)
        {
            this.look = look;
            this.index = index;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            look.Serialize(writer);
            writer.WriteSByte(index);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            look = new Types.EntityLook();
            look.Deserialize(reader);
            index = reader.ReadSByte();
            if (index < 0)
                throw new Exception("Forbidden value on index = " + index + ", it doesn't respect the following condition : index < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return look.GetSerializationSize() + sizeof(sbyte);
        }
        
    }
    
}