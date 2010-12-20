using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LockableCodeResultMessage : Message
	{
		public const uint protocolId = 5672;
		internal Boolean _isInitialized = false;
		public Boolean success = false;
		
		public LockableCodeResultMessage()
		{
		}
		
		public LockableCodeResultMessage(Boolean arg1)
			: this()
		{
			initLockableCodeResultMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5672;
		}
		
		public LockableCodeResultMessage initLockableCodeResultMessage(Boolean arg1 = false)
		{
			this.success = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.success = false;
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
			this.serializeAs_LockableCodeResultMessage(arg1);
		}
		
		public void serializeAs_LockableCodeResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.success);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LockableCodeResultMessage(arg1);
		}
		
		public void deserializeAs_LockableCodeResultMessage(BigEndianReader arg1)
		{
			this.success = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
