using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class NotificationByServerMessage : Message
	{
		public const uint protocolId = 6103;
		internal Boolean _isInitialized = false;
		public uint id = 0;
		public List<String> parameters;
		
		public NotificationByServerMessage()
		{
			this.parameters = new List<String>();
		}
		
		public NotificationByServerMessage(uint arg1, List<String> arg2)
			: this()
		{
			initNotificationByServerMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6103;
		}
		
		public NotificationByServerMessage initNotificationByServerMessage(uint arg1 = 0, List<String> arg2 = null)
		{
			this.id = arg1;
			this.parameters = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
			this.parameters = new List<String>();
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
			this.serializeAs_NotificationByServerMessage(arg1);
		}
		
		public void serializeAs_NotificationByServerMessage(BigEndianWriter arg1)
		{
			if ( this.id < 0 || this.id > 65535 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteShort((short)this.id);
			arg1.WriteShort((short)this.parameters.Count);
			var loc1 = 0;
			while ( loc1 < this.parameters.Count )
			{
				arg1.WriteUTF((string)this.parameters[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NotificationByServerMessage(arg1);
		}
		
		public void deserializeAs_NotificationByServerMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.id = (uint)arg1.ReadUShort();
			if ( this.id < 0 || this.id > 65535 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of NotificationByServerMessage.id.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadUTF();
				this.parameters.Add((String)loc3);
				++loc2;
			}
		}
		
	}
}
