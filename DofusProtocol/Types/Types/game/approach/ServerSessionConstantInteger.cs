

// Generated on 04/24/2015 03:38:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ServerSessionConstantInteger : ServerSessionConstant
    {
        public const short Id = 433;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int value;
        
        public ServerSessionConstantInteger()
        {
        }
        
        public ServerSessionConstantInteger(short id, int value)
         : base(id)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadInt();
        }
        
        
    }
    
}