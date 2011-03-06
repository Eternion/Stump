using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AcquaintanceSearchErrorMessage : Message
	{
		public const uint protocolId = 6143;
		internal Boolean _isInitialized = false;
		public uint reason = 0;
		
		public AcquaintanceSearchErrorMessage()
		{
		}
		
		public AcquaintanceSearchErrorMessage(uint arg1)
			: this()
		{
			initAcquaintanceSearchErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6143;
		}
		
		public AcquaintanceSearchErrorMessage initAcquaintanceSearchErrorMessage(uint arg1 = 0)
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
			this.serializeAs_AcquaintanceSearchErrorMessage(arg1);
		}
		
		public void serializeAs_AcquaintanceSearchErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AcquaintanceSearchErrorMessage(arg1);
		}
		
		public void deserializeAs_AcquaintanceSearchErrorMessage(BigEndianReader arg1)
		{
			this.reason = (uint)arg1.ReadByte();
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element of AcquaintanceSearchErrorMessage.reason.");
			}
		}
		
	}
}
