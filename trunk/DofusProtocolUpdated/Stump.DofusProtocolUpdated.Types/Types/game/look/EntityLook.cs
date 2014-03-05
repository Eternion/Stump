

// Generated on 03/05/2014 20:34:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class EntityLook
    {
        public const short Id = 55;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public EntityLook()
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