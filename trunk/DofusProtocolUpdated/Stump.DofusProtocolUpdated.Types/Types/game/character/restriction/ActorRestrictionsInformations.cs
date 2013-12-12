

// Generated on 12/12/2013 16:57:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ActorRestrictionsInformations
    {
        public const short Id = 204;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public ActorRestrictionsInformations()
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