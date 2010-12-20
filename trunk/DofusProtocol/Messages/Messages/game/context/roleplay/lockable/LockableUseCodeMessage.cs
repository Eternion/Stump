using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LockableUseCodeMessage : Message
	{
		public const uint protocolId = 5667;
		internal Boolean _isInitialized = false;
		public String code = "";
		
		public LockableUseCodeMessage()
		{
		}
		
		public LockableUseCodeMessage(String arg1)
			: this()
		{
			initLockableUseCodeMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5667;
		}
		
		public LockableUseCodeMessage initLockableUseCodeMessage(String arg1 = "")
		{
			this.code = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.code = "";
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
			this.serializeAs_LockableUseCodeMessage(arg1);
		}
		
		public void serializeAs_LockableUseCodeMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.code);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LockableUseCodeMessage(arg1);
		}
		
		public void deserializeAs_LockableUseCodeMessage(BigEndianReader arg1)
		{
			this.code = (String)arg1.ReadUTF();
		}
		
	}
}
