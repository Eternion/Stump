using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ShortcutBarRemovedMessage : Message
	{
		public const uint protocolId = 6224;
		internal Boolean _isInitialized = false;
		public uint slot = 0;
		
		public ShortcutBarRemovedMessage()
		{
		}
		
		public ShortcutBarRemovedMessage(uint arg1)
			: this()
		{
			initShortcutBarRemovedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6224;
		}
		
		public ShortcutBarRemovedMessage initShortcutBarRemovedMessage(uint arg1 = 0)
		{
			this.slot = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.slot = 0;
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
			this.serializeAs_ShortcutBarRemovedMessage(arg1);
		}
		
		public void serializeAs_ShortcutBarRemovedMessage(BigEndianWriter arg1)
		{
			if ( this.slot < 0 || this.slot > 99 )
			{
				throw new Exception("Forbidden value (" + this.slot + ") on element slot.");
			}
			arg1.WriteInt((int)this.slot);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutBarRemovedMessage(arg1);
		}
		
		public void deserializeAs_ShortcutBarRemovedMessage(BigEndianReader arg1)
		{
			this.slot = (uint)arg1.ReadInt();
			if ( this.slot < 0 || this.slot > 99 )
			{
				throw new Exception("Forbidden value (" + this.slot + ") on element of ShortcutBarRemovedMessage.slot.");
			}
		}
		
	}
}
