

// Generated on 04/19/2016 10:17:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TreasureHuntStep
    {
        public const short Id = 463;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        
        public TreasureHuntStep()
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