

// Generated on 07/26/2013 22:51:10
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultAdditionalData
    {
        public const short Id = 191;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public FightResultAdditionalData()
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