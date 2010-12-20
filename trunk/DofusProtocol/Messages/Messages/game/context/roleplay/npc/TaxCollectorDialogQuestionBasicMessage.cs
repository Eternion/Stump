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
		public String guildName = "";
		
		public TaxCollectorDialogQuestionBasicMessage()
		{
		}
		
		public TaxCollectorDialogQuestionBasicMessage(String arg1)
			: this()
		{
			initTaxCollectorDialogQuestionBasicMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5619;
		}
		
		public TaxCollectorDialogQuestionBasicMessage initTaxCollectorDialogQuestionBasicMessage(String arg1 = "")
		{
			this.guildName = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.guildName = "";
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
			arg1.WriteUTF((string)this.guildName);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorDialogQuestionBasicMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorDialogQuestionBasicMessage(BigEndianReader arg1)
		{
			this.guildName = (String)arg1.ReadUTF();
		}
		
	}
}
