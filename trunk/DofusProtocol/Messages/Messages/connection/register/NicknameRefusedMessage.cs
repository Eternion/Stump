using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class NicknameRefusedMessage : Message
	{
		public const uint protocolId = 5638;
		internal Boolean _isInitialized = false;
		public uint reason = 99;
		
		public NicknameRefusedMessage()
		{
		}
		
		public NicknameRefusedMessage(uint arg1)
			: this()
		{
			initNicknameRefusedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5638;
		}
		
		public NicknameRefusedMessage initNicknameRefusedMessage(uint arg1 = 99)
		{
			this.reason = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.reason = 99;
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
			this.serializeAs_NicknameRefusedMessage(arg1);
		}
		
		public void serializeAs_NicknameRefusedMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NicknameRefusedMessage(arg1);
		}
		
		public void deserializeAs_NicknameRefusedMessage(BigEndianReader arg1)
		{
			this.reason = (uint)arg1.ReadByte();
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element of NicknameRefusedMessage.reason.");
			}
		}
		
	}
}
