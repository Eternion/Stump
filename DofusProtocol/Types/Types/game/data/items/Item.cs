

// Generated on 02/18/2015 11:05:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class Item
    {
        public const short Id = 7;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public Item()
        {
        }
        
        
        public virtual void Serialize(IDataWriter writer)
        {
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
        }
        
        
    }
    
}