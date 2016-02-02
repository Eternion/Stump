

// Generated on 02/02/2016 14:14:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class StatisticDataShort : StatisticData
    {
        public const short Id = 488;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short value;
        
        public StatisticDataShort()
        {
        }
        
        public StatisticDataShort(short actionId, short value)
         : base(actionId)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadShort();
        }
        
        
    }
    
}