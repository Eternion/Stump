using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class FriendAddedMessage : Message
	{
		public const uint protocolId = 5599;
		internal Boolean _isInitialized = false;
		public FriendInformations friendAdded;
		
		public FriendAddedMessage()
		{
			this.friendAdded = new FriendInformations();
		}
		
		public FriendAddedMessage(FriendInformations arg1)
			: this()
		{
			initFriendAddedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5599;
		}
		
		public FriendAddedMessage initFriendAddedMessage(FriendInformations arg1 = null)
		{
			this.friendAdded = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.friendAdded = new FriendInformations();
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
			this.serializeAs_FriendAddedMessage(arg1);
		}
		
		public void serializeAs_FriendAddedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.friendAdded.getTypeId());
			this.friendAdded.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendAddedMessage(arg1);
		}
		
		public void deserializeAs_FriendAddedMessage(BigEndianReader arg1)
		{
			var loc1 = (ushort)arg1.ReadUShort();
			this.friendAdded = ProtocolTypeManager.GetInstance<FriendInformations>((uint)loc1);
			this.friendAdded.deserialize(arg1);
		}
		
	}
}
