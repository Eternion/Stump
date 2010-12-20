using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SpellForgetUIMessage : Message
	{
		public const uint protocolId = 5565;
		internal Boolean _isInitialized = false;
		public Boolean open = false;
		
		public SpellForgetUIMessage()
		{
		}
		
		public SpellForgetUIMessage(Boolean arg1)
			: this()
		{
			initSpellForgetUIMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5565;
		}
		
		public SpellForgetUIMessage initSpellForgetUIMessage(Boolean arg1 = false)
		{
			this.open = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.open = false;
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
			this.serializeAs_SpellForgetUIMessage(arg1);
		}
		
		public void serializeAs_SpellForgetUIMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.open);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellForgetUIMessage(arg1);
		}
		
		public void deserializeAs_SpellForgetUIMessage(BigEndianReader arg1)
		{
			this.open = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
