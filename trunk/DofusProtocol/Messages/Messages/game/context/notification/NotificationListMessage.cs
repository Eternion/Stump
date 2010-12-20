using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class NotificationListMessage : Message
	{
		public const uint protocolId = 6087;
		internal Boolean _isInitialized = false;
		public List<int> flags;
		
		public NotificationListMessage()
		{
			this.flags = new List<int>();
		}
		
		public NotificationListMessage(List<int> arg1)
			: this()
		{
			initNotificationListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6087;
		}
		
		public NotificationListMessage initNotificationListMessage(List<int> arg1)
		{
			this.flags = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.flags = new List<int>();
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
			this.serializeAs_NotificationListMessage(arg1);
		}
		
		public void serializeAs_NotificationListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.flags.Count);
			var loc1 = 0;
			while ( loc1 < this.flags.Count )
			{
				arg1.WriteInt((int)this.flags[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NotificationListMessage(arg1);
		}
		
		public void deserializeAs_NotificationListMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadInt();
				this.flags.Add((int)loc3);
				++loc2;
			}
		}
		
	}
}
