using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ShortcutBarSwapErrorMessage : Message
	{
		public const uint protocolId = 6226;
		internal Boolean _isInitialized = false;
		public uint error = 0;
		
		public ShortcutBarSwapErrorMessage()
		{
		}
		
		public ShortcutBarSwapErrorMessage(uint arg1)
			: this()
		{
			initShortcutBarSwapErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6226;
		}
		
		public ShortcutBarSwapErrorMessage initShortcutBarSwapErrorMessage(uint arg1 = 0)
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
			this.serializeAs_ShortcutBarSwapErrorMessage(arg1);
		}
		
		public void serializeAs_ShortcutBarSwapErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.error);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutBarSwapErrorMessage(arg1);
		}
		
		public void deserializeAs_ShortcutBarSwapErrorMessage(BigEndianReader arg1)
		{
			this.error = (uint)arg1.ReadByte();
			if ( this.error < 0 )
			{
				throw new Exception("Forbidden value (" + this.error + ") on element of ShortcutBarSwapErrorMessage.error.");
			}
		}
		
	}
}
