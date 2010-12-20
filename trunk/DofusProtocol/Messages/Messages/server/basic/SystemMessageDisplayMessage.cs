using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SystemMessageDisplayMessage : Message
	{
		public const uint protocolId = 189;
		internal Boolean _isInitialized = false;
		public Boolean hangUp = false;
		public uint msgId = 0;
		public List<String> parameters;
		
		public SystemMessageDisplayMessage()
		{
			this.parameters = new List<String>();
		}
		
		public SystemMessageDisplayMessage(Boolean arg1, uint arg2, List<String> arg3)
			: this()
		{
			initSystemMessageDisplayMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 189;
		}
		
		public SystemMessageDisplayMessage initSystemMessageDisplayMessage(Boolean arg1 = false, uint arg2 = 0, List<String> arg3 = null)
		{
			this.hangUp = arg1;
			this.msgId = arg2;
			this.parameters = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.hangUp = false;
			this.msgId = 0;
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
			this.serializeAs_SystemMessageDisplayMessage(arg1);
		}
		
		public void serializeAs_SystemMessageDisplayMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.hangUp);
			if ( this.msgId < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgId + ") on element msgId.");
			}
			arg1.WriteShort((short)this.msgId);
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
			this.deserializeAs_SystemMessageDisplayMessage(arg1);
		}
		
		public void deserializeAs_SystemMessageDisplayMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.hangUp = (Boolean)arg1.ReadBoolean();
			this.msgId = (uint)arg1.ReadShort();
			if ( this.msgId < 0 )
			{
				throw new Exception("Forbidden value (" + this.msgId + ") on element of SystemMessageDisplayMessage.msgId.");
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
