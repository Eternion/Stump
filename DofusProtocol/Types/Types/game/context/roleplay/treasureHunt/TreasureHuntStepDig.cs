

// Generated on 04/19/2016 10:17:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TreasureHuntStepDig : TreasureHuntStep
    {
        public const short Id = 465;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public TreasureHuntStepDig()
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
        
        
    }
    
}