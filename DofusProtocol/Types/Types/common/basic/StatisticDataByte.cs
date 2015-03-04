

// Generated on 02/19/2015 12:10:35
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
        
        public StatisticDataByte(short actionId, sbyte value)
         : base(actionId)
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