using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorDialogQuestionBasicMessage : Message
	{
		public const uint protocolId = 5619;
		internal Boolean _isInitialized = false;
		public BasicGuildInformations guildInfo;
		
		public TaxCollectorDialogQuestionBasicMessage()
		{
			this.guildInfo = new BasicGuildInformations();
		}
		
		public TaxCollectorDialogQuestionBasicMessage(BasicGuildInformations arg1)
			: this()
		{
			initTaxCollectorDialogQuestionBasicMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5619;
		}
		
		public TaxCollectorDialogQuestionBasicMessage initTaxCollectorDialogQuestionBasicMessage(BasicGuildInformations arg1 = null)
		{
			this.guildInfo = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.guildInfo = new BasicGuildInformations();
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
			this.serializeAs_TaxCollectorDialogQuestionBasicMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorDialogQuestionBasicMessage(BigEndianWriter arg1)
		{
			this.guildInfo.serializeAs_BasicGuildInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorDialogQuestionBasicMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorDialogQuestionBasicMessage(BigEndianReader arg1)
		{
			this.guildInfo = new BasicGuildInformations();
			this.guildInfo.deserialize(arg1);
		}
		
	}
}
