

// Generated on 10/30/2016 16:20:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class DareCriteria
    {
        public const short Id = 501;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte type;
        public IEnumerable<int> params;
        
        public DareCriteria()
        {
        }
        
        public DareCriteria(sbyte type, IEnumerable<int> params)
        {
            this.type = type;
            this.params = params;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(type);
            var params_before = writer.Position;
            var params_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in params)
            {
                 writer.WriteInt(entry);
                 params_count++;
            }
            var params_after = writer.Position;
            writer.Seek((int)params_before);
            writer.WriteUShort((ushort)params_count);
            writer.Seek((int)params_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            var limit = reader.ReadUShort();
            var params_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 params_[i] = reader.ReadInt();
            }
            params = params_;
        }
        
        
    }
    
}