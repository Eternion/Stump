using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class FriendAddFailureMessage : Message
	{
		public const uint protocolId = 5600;
		internal Boolean _isInitialized = false;
		public uint reason = 0;
		
		public FriendAddFailureMessage()
		{
		}
		
		public FriendAddFailureMessage(uint arg1)
			: this()
		{
			initFriendAddFailureMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5600;
		}
		
		public FriendAddFailureMessage initFriendAddFailureMessage(uint arg1 = 0)
		{
			this.reason = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.reason = 0;
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
			this.serializeAs_FriendAddFailureMessage(arg1);
		}
		
		public void serializeAs_FriendAddFailureMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendAddFailureMessage(arg1);
		}
		
		public void deserializeAs_FriendAddFailureMessage(BigEndianReader arg1)
		{
			this.reason = (uint)arg1.ReadByte();
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element of FriendAddFailureMessage.reason.");
			}
		}
		
	}
}
