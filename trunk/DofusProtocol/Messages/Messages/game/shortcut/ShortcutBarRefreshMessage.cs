using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ShortcutBarRefreshMessage : Message
	{
		public const uint protocolId = 6229;
		internal Boolean _isInitialized = false;
		public uint barType = 0;
		public Shortcut shortcut;
		
		public ShortcutBarRefreshMessage()
		{
			this.shortcut = new Shortcut();
		}
		
		public ShortcutBarRefreshMessage(uint arg1, Shortcut arg2)
			: this()
		{
			initShortcutBarRefreshMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6229;
		}
		
		public ShortcutBarRefreshMessage initShortcutBarRefreshMessage(uint arg1 = 0, Shortcut arg2 = null)
		{
			this.barType = arg1;
			this.shortcut = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.barType = 0;
			this.shortcut = new Shortcut();
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
			this.serializeAs_ShortcutBarRefreshMessage(arg1);
		}
		
		public void serializeAs_ShortcutBarRefreshMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.barType);
			this.shortcut.serializeAs_Shortcut(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutBarRefreshMessage(arg1);
		}
		
		public void deserializeAs_ShortcutBarRefreshMessage(BigEndianReader arg1)
		{
			this.barType = (uint)arg1.ReadByte();
			if ( this.barType < 0 )
			{
				throw new Exception("Forbidden value (" + this.barType + ") on element of ShortcutBarRefreshMessage.barType.");
			}
			this.shortcut = new Shortcut();
			this.shortcut.deserialize(arg1);
		}
		
	}
}
