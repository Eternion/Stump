using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AcquaintanceSearchMessage : Message
	{
		public const uint protocolId = 6144;
		internal Boolean _isInitialized = false;
		public String nickname = "";
		
		public AcquaintanceSearchMessage()
		{
		}
		
		public AcquaintanceSearchMessage(String arg1)
			: this()
		{
			initAcquaintanceSearchMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6144;
		}
		
		public AcquaintanceSearchMessage initAcquaintanceSearchMessage(String arg1 = "")
		{
			this.nickname = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nickname = "";
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
			this.serializeAs_AcquaintanceSearchMessage(arg1);
		}
		
		public void serializeAs_AcquaintanceSearchMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.nickname);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AcquaintanceSearchMessage(arg1);
		}
		
		public void deserializeAs_AcquaintanceSearchMessage(BigEndianReader arg1)
		{
			this.nickname = (String)arg1.ReadUTF();
		}
		
	}
}
