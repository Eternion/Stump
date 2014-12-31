

// Generated on 12/29/2014 21:14:33
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TreasureHuntStepFight : TreasureHuntStep
    {
        public const short Id = 462;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public TreasureHuntStepFight()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize();
        }
        
    }
    
}