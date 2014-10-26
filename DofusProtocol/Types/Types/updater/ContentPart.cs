

// Generated on 10/26/2014 23:30:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ContentPart
    {
        public const short Id = 350;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public string id;
        public sbyte state;
        
        public ContentPart()
        {
        }
        
        public ContentPart(string id, sbyte state)
        {
            this.id = id;
            this.state = state;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(id);
            writer.WriteSByte(state);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadUTF();
            state = reader.ReadSByte();
            if (state < 0)
                throw new Exception("Forbidden value on state = " + state + ", it doesn't respect the following condition : state < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(id) + sizeof(sbyte);
        }
        
    }
    
}