

// Generated on 04/19/2016 10:17:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class StatisticDataByte : StatisticData
    {
        public const short Id = 486;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte value;
        
        public StatisticDataByte()
        {
        }
        
        public StatisticDataByte(sbyte value)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadSByte();
        }
        
        
    }
    
}