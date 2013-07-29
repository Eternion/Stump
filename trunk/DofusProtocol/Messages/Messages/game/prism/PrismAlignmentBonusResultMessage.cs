

// Generated on 07/29/2013 23:08:32
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
        
        public override int GetSerializationSize()
        {
            return alignmentBonus.GetSerializationSize();
        }
        
    }
    
}