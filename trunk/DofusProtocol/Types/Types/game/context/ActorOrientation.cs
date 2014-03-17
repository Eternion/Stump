

// Generated on 03/02/2014 20:42:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ActorOrientation
    {
        public const short Id = 353;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        public sbyte direction;
        
        public ActorOrientation()
        {
        }
        
        public ActorOrientation(int id, sbyte direction)
        {
            this.id = id;
            this.direction = direction;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteSByte(direction);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(sbyte);
        }
        
    }
    
}