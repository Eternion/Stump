

// Generated on 04/19/2016 10:17:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class StatisticDataInt : StatisticData
    {
        public const short Id = 485;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int value;
        
        public StatisticDataInt()
        {
        }
        
        public StatisticDataInt(int value)
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