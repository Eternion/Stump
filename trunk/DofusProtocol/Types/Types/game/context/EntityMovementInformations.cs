

// Generated on 08/11/2013 11:29:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class EntityMovementInformations
    {
        public const short Id = 63;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int id;
        public IEnumerable<sbyte> steps;
        
        public EntityMovementInformations()
        {
        }
        
        public EntityMovementInformations(int id, IEnumerable<sbyte> steps)
        {
            this.id = id;
            this.steps = steps;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
            writer.WriteUShort((ushort)steps.Count());
            foreach (var entry in steps)
            {
                 writer.WriteSByte(entry);
            }
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
            var limit = reader.ReadUShort();
            steps = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (steps as sbyte[])[i] = reader.ReadSByte();
            }
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + steps.Sum(x => sizeof(sbyte));
        }
        
    }
    
}