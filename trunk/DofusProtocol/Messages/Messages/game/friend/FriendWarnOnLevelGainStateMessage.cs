using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class FriendWarnOnLevelGainStateMessage : Message
	{
		public const uint protocolId = 6078;
		internal Boolean _isInitialized = false;
		public Boolean enable = false;
		
		public FriendWarnOnLevelGainStateMessage()
		{
		}
		
		public FriendWarnOnLevelGainStateMessage(Boolean arg1)
			: this()
		{
			initFriendWarnOnLevelGainStateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6078;
		}
		
		public FriendWarnOnLevelGainStateMessage initFriendWarnOnLevelGainStateMessage(Boolean arg1 = false)
		{
			this.enable = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.enable = false;
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
			this.serializeAs_FriendWarnOnLevelGainStateMessage(arg1);
		}
		
		public void serializeAs_FriendWarnOnLevelGainStateMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.enable);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendWarnOnLevelGainStateMessage(arg1);
		}
		
		public void deserializeAs_FriendWarnOnLevelGainStateMessage(BigEndianReader arg1)
		{
			this.enable = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
