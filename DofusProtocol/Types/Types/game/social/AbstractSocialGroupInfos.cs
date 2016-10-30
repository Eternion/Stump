

// Generated on 10/30/2016 16:21:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class AbstractSocialGroupInfos
    {
        public const short Id = 416;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public AbstractSocialGroupInfos()
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