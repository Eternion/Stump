

// Generated on 12/20/2015 17:30:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class StatisticDataBoolean : StatisticData
    {
        public const short Id = 482;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public bool value;
        
        public StatisticDataBoolean()
        {
        }
        
        public StatisticDataBoolean(short actionId, bool value)
         : base(actionId)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteBoolean(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadBoolean();
        }
        
        
    }
    
}