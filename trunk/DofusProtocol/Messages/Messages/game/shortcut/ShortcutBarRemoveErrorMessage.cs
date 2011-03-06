using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ShortcutBarRemoveErrorMessage : Message
	{
		public const uint protocolId = 6222;
		internal Boolean _isInitialized = false;
		public uint error = 0;
		
		public ShortcutBarRemoveErrorMessage()
		{
		}
		
		public ShortcutBarRemoveErrorMessage(uint arg1)
			: this()
		{
			initShortcutBarRemoveErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6222;
		}
		
		public ShortcutBarRemoveErrorMessage initShortcutBarRemoveErrorMessage(uint arg1 = 0)
		{
			this.error = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.error = 0;
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
			this.serializeAs_ShortcutBarRemoveErrorMessage(arg1);
		}
		
		public void serializeAs_ShortcutBarRemoveErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.error);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutBarRemoveErrorMessage(arg1);
		}
		
		public void deserializeAs_ShortcutBarRemoveErrorMessage(BigEndianReader arg1)
		{
			this.error = (uint)arg1.ReadByte();
			if ( this.error < 0 )
			{
				throw new Exception("Forbidden value (" + this.error + ") on element of ShortcutBarRemoveErrorMessage.error.");
			}
		}
		
	}
}
