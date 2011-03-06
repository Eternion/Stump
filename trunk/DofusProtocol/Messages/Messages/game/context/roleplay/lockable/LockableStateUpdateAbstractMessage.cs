using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LockableStateUpdateAbstractMessage : Message
	{
		public const uint protocolId = 5671;
		internal Boolean _isInitialized = false;
		public Boolean locked = false;
		
		public LockableStateUpdateAbstractMessage()
		{
		}
		
		public LockableStateUpdateAbstractMessage(Boolean arg1)
			: this()
		{
			initLockableStateUpdateAbstractMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5671;
		}
		
		public LockableStateUpdateAbstractMessage initLockableStateUpdateAbstractMessage(Boolean arg1 = false)
		{
			this.locked = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.locked = false;
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
			this.serializeAs_LockableStateUpdateAbstractMessage(arg1);
		}
		
		public void serializeAs_LockableStateUpdateAbstractMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.locked);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LockableStateUpdateAbstractMessage(arg1);
		}
		
		public void deserializeAs_LockableStateUpdateAbstractMessage(BigEndianReader arg1)
		{
			this.locked = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
