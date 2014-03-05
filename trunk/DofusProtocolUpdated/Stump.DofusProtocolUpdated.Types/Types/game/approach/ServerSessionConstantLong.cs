

// Generated on 03/05/2014 20:34:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ServerSessionConstantLong : ServerSessionConstant
    {
        public const short Id = 429;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public double value;
        
        public ServerSessionConstantLong()
        {
        }
        
        public ServerSessionConstantLong(short id, double value)
         : base(id)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadDouble();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(double);
        }
        
    }
    
}