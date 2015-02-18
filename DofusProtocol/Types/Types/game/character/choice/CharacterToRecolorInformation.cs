

// Generated on 02/18/2015 11:05:50
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterToRecolorInformation : AbstractCharacterToRefurbishInformation
    {
        public const short Id = 212;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public CharacterToRecolorInformation()
        {
        }
        
        public CharacterToRecolorInformation(int id, IEnumerable<int> colors, int cosmeticId)
         : base(id, colors, cosmeticId)
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