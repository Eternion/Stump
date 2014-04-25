

// Generated on 03/02/2014 20:43:02
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
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}