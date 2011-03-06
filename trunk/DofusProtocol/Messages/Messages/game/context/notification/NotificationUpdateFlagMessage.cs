using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class NotificationUpdateFlagMessage : Message
	{
		public const uint protocolId = 6090;
		internal Boolean _isInitialized = false;
		public uint index = 0;
		
		public NotificationUpdateFlagMessage()
		{
		}
		
		public NotificationUpdateFlagMessage(uint arg1)
			: this()
		{
			initNotificationUpdateFlagMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6090;
		}
		
		public NotificationUpdateFlagMessage initNotificationUpdateFlagMessage(uint arg1 = 0)
		{
			this.index = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.index = 0;
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
			this.serializeAs_NotificationUpdateFlagMessage(arg1);
		}
		
		public void serializeAs_NotificationUpdateFlagMessage(BigEndianWriter arg1)
		{
			if ( this.index < 0 )
			{
				throw new Exception("Forbidden value (" + this.index + ") on element index.");
			}
			arg1.WriteShort((short)this.index);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NotificationUpdateFlagMessage(arg1);
		}
		
		public void deserializeAs_NotificationUpdateFlagMessage(BigEndianReader arg1)
		{
			this.index = (uint)arg1.ReadShort();
			if ( this.index < 0 )
			{
				throw new Exception("Forbidden value (" + this.index + ") on element of NotificationUpdateFlagMessage.index.");
			}
		}
		
	}
}
