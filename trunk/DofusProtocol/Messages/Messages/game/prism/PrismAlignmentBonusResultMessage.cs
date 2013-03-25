
// Generated on 03/25/2013 19:24:23
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismAlignmentBonusResultMessage : Message
    {
        public const uint Id = 5842;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AlignmentBonusInformations alignmentBonus;
        
        public PrismAlignmentBonusResultMessage()
        {
        }
        
        public PrismAlignmentBonusResultMessage(Types.AlignmentBonusInformations alignmentBonus)
        {
            this.alignmentBonus = alignmentBonus;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            alignmentBonus.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            alignmentBonus = new Types.AlignmentBonusInformations();
            alignmentBonus.Deserialize(reader);
        }
        
    }
    
}