

// Generated on 09/01/2015 10:48:36
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