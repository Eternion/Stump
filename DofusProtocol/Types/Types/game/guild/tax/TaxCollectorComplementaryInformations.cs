

// Generated on 09/01/2014 15:52:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TaxCollectorComplementaryInformations
    {
        public const short Id = 448;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public TaxCollectorComplementaryInformations()
        {
        }
        
        
        public virtual void Serialize(IDataWriter writer)
        {
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
        }
        
        public virtual int GetSerializationSize()
        {
            return 0;
        }
        
    }
    
}