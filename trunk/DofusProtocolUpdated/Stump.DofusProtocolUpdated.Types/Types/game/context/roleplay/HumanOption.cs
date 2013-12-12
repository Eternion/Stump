

// Generated on 12/12/2013 16:57:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HumanOption
    {
        public const short Id = 406;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public HumanOption()
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
            ;
        }
        
    }
    
}