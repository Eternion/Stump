using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class FriendSetWarnOnLevelGainMessage : Message
	{
		public const uint protocolId = 6077;
		internal Boolean _isInitialized = false;
		public Boolean enable = false;
		
		public FriendSetWarnOnLevelGainMessage()
		{
		}
		
		public FriendSetWarnOnLevelGainMessage(Boolean arg1)
			: this()
		{
			initFriendSetWarnOnLevelGainMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6077;
		}
		
		public FriendSetWarnOnLevelGainMessage initFriendSetWarnOnLevelGainMessage(Boolean arg1 = false)
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
			this.serializeAs_FriendSetWarnOnLevelGainMessage(arg1);
		}
		
		public void serializeAs_FriendSetWarnOnLevelGainMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.enable);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendSetWarnOnLevelGainMessage(arg1);
		}
		
		public void deserializeAs_FriendSetWarnOnLevelGainMessage(BigEndianReader arg1)
		{
			this.enable = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
