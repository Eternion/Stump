using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SelectedServerDataMessage : Message
	{
		public const uint protocolId = 42;
		internal Boolean _isInitialized = false;
		public int serverId = 0;
		public String address = "";
		public uint port = 0;
		public Boolean canCreateNewCharacter = false;
		public String ticket = "";
		
		public SelectedServerDataMessage()
		{
		}
		
		public SelectedServerDataMessage(int arg1, String arg2, uint arg3, Boolean arg4, String arg5)
			: this()
		{
			initSelectedServerDataMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 42;
		}
		
		public SelectedServerDataMessage initSelectedServerDataMessage(int arg1 = 0, String arg2 = "", uint arg3 = 0, Boolean arg4 = false, String arg5 = "")
		{
			this.serverId = arg1;
			this.address = arg2;
			this.port = arg3;
			this.canCreateNewCharacter = arg4;
			this.ticket = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.serverId = 0;
			this.address = "";
			this.port = 0;
			this.canCreateNewCharacter = false;
			this.ticket = "";
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
			this.serializeAs_SelectedServerDataMessage(arg1);
		}
		
		public void serializeAs_SelectedServerDataMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.serverId);
			arg1.WriteUTF((string)this.address);
			if ( this.port < 0 || this.port > 65535 )
			{
				throw new Exception("Forbidden value (" + this.port + ") on element port.");
			}
			arg1.WriteShort((short)this.port);
			arg1.WriteBoolean(this.canCreateNewCharacter);
			arg1.WriteUTF((string)this.ticket);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SelectedServerDataMessage(arg1);
		}
		
		public void deserializeAs_SelectedServerDataMessage(BigEndianReader arg1)
		{
			this.serverId = (int)arg1.ReadShort();
			this.address = (String)arg1.ReadUTF();
			this.port = (uint)arg1.ReadUShort();
			if ( this.port < 0 || this.port > 65535 )
			{
				throw new Exception("Forbidden value (" + this.port + ") on element of SelectedServerDataMessage.port.");
			}
			this.canCreateNewCharacter = (Boolean)arg1.ReadBoolean();
			this.ticket = (String)arg1.ReadUTF();
		}
		
	}
}
