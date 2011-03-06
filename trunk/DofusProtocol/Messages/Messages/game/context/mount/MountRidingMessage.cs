using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountRidingMessage : Message
	{
		public const uint protocolId = 5967;
		internal Boolean _isInitialized = false;
		public Boolean isRiding = false;
		
		public MountRidingMessage()
		{
		}
		
		public MountRidingMessage(Boolean arg1)
			: this()
		{
			initMountRidingMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5967;
		}
		
		public MountRidingMessage initMountRidingMessage(Boolean arg1 = false)
		{
			this.isRiding = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.isRiding = false;
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
			this.serializeAs_MountRidingMessage(arg1);
		}
		
		public void serializeAs_MountRidingMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.isRiding);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountRidingMessage(arg1);
		}
		
		public void deserializeAs_MountRidingMessage(BigEndianReader arg1)
		{
			this.isRiding = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
