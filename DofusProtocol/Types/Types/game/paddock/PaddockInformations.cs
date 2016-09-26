

// Generated on 09/26/2016 01:50:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class PaddockInformations
    {
        public const short Id = 132;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short maxOutdoorMount;
        public short maxItems;
        
        public PaddockInformations()
        {
        }
        
        public PaddockInformations(short maxOutdoorMount, short maxItems)
        {
            this.maxOutdoorMount = maxOutdoorMount;
            this.maxItems = maxItems;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(maxOutdoorMount);
            writer.WriteVarShort(maxItems);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            maxOutdoorMount = reader.ReadVarShort();
            if (maxOutdoorMount < 0)
                throw new Exception("Forbidden value on maxOutdoorMount = " + maxOutdoorMount + ", it doesn't respect the following condition : maxOutdoorMount < 0");
            maxItems = reader.ReadVarShort();
            if (maxItems < 0)
                throw new Exception("Forbidden value on maxItems = " + maxItems + ", it doesn't respect the following condition : maxItems < 0");
        }
        
        
    }
    
}