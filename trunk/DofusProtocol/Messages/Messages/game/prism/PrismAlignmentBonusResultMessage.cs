// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PrismAlignmentBonusResultMessage.xml' the '03/10/2011 12:47:09'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PrismAlignmentBonusResultMessage : Message
	{
		public const uint Id = 5842;
		public override uint MessageId
		{
			get
			{
				return 5842;
			}
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
