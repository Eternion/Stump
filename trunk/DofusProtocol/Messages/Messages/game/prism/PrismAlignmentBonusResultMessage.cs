using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismAlignmentBonusResultMessage : Message
	{
		public const uint protocolId = 5842;
		internal Boolean _isInitialized = false;
		public AlignmentBonusInformations alignmentBonus;
		
		public PrismAlignmentBonusResultMessage()
		{
			this.alignmentBonus = new AlignmentBonusInformations();
		}
		
		public PrismAlignmentBonusResultMessage(AlignmentBonusInformations arg1)
			: this()
		{
			initPrismAlignmentBonusResultMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5842;
		}
		
		public PrismAlignmentBonusResultMessage initPrismAlignmentBonusResultMessage(AlignmentBonusInformations arg1 = null)
		{
			this.alignmentBonus = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.alignmentBonus = new AlignmentBonusInformations();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PrismAlignmentBonusResultMessage(arg1);
		}
		
		public void serializeAs_PrismAlignmentBonusResultMessage(BigEndianWriter arg1)
		{
			this.alignmentBonus.serializeAs_AlignmentBonusInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismAlignmentBonusResultMessage(arg1);
		}
		
		public void deserializeAs_PrismAlignmentBonusResultMessage(BigEndianReader arg1)
		{
			this.alignmentBonus = new AlignmentBonusInformations();
			this.alignmentBonus.deserialize(arg1);
		}
		
	}
}
