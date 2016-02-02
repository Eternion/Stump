

// Generated on 02/02/2016 14:14:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class StatisticDataString : StatisticData
    {
        public const short Id = 487;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public string value;
        
        public StatisticDataString()
        {
        }
        
        public StatisticDataString(short actionId, string value)
         : base(actionId)
        {
            this.value = value;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteUTF(value);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            value = reader.ReadUTF();
        }
        
        
    }
    
}