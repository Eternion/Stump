

// Generated on 04/24/2015 03:38:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class CharacterToRelookInformation : AbstractCharacterToRefurbishInformation
    {
        public const short Id = 399;
        public override short TypeId
        {
            get { return Id; }
        }
        
        
        public CharacterToRelookInformation()
        {
        }
        
        public CharacterToRelookInformation(int id, IEnumerable<int> colors, int cosmeticId)
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