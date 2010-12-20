using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class FriendAddRequestMessage : Message
	{
		public const uint protocolId = 4004;
		internal Boolean _isInitialized = false;
		public String name = "";
		
		public FriendAddRequestMessage()
		{
		}
		
		public FriendAddRequestMessage(String arg1)
			: this()
		{
			initFriendAddRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 4004;
		}
		
		public FriendAddRequestMessage initFriendAddRequestMessage(String arg1 = "")
		{
			this.name = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.name = "";
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
			this.serializeAs_FriendAddRequestMessage(arg1);
		}
		
		public void serializeAs_FriendAddRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendAddRequestMessage(arg1);
		}
		
		public void deserializeAs_FriendAddRequestMessage(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
