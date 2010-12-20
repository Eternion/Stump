using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LockableChangeCodeMessage : Message
	{
		public const uint protocolId = 5666;
		internal Boolean _isInitialized = false;
		public String code = "";
		
		public LockableChangeCodeMessage()
		{
		}
		
		public LockableChangeCodeMessage(String arg1)
			: this()
		{
			initLockableChangeCodeMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5666;
		}
		
		public LockableChangeCodeMessage initLockableChangeCodeMessage(String arg1 = "")
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
			this.serializeAs_LockableChangeCodeMessage(arg1);
		}
		
		public void serializeAs_LockableChangeCodeMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.code);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LockableChangeCodeMessage(arg1);
		}
		
		public void deserializeAs_LockableChangeCodeMessage(BigEndianReader arg1)
		{
			this.code = (String)arg1.ReadUTF();
		}
		
	}
}
