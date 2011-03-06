using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class FriendUpdateMessage : Message
	{
		public const uint protocolId = 5924;
		internal Boolean _isInitialized = false;
		public FriendInformations friendUpdated;
		
		public FriendUpdateMessage()
		{
			this.friendUpdated = new FriendInformations();
		}
		
		public FriendUpdateMessage(FriendInformations arg1)
			: this()
		{
			initFriendUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5924;
		}
		
		public FriendUpdateMessage initFriendUpdateMessage(FriendInformations arg1 = null)
		{
			this.friendUpdated = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.friendUpdated = new FriendInformations();
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
			this.serializeAs_FriendUpdateMessage(arg1);
		}
		
		public void serializeAs_FriendUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.friendUpdated.getTypeId());
			this.friendUpdated.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendUpdateMessage(arg1);
		}
		
		public void deserializeAs_FriendUpdateMessage(BigEndianReader arg1)
		{
			var loc1 = (ushort)arg1.ReadUShort();
			this.friendUpdated = ProtocolTypeManager.GetInstance<FriendInformations>((uint)loc1);
			this.friendUpdated.deserialize(arg1);
		}
		
	}
}
