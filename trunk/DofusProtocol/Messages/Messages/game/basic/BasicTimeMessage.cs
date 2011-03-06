using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class BasicTimeMessage : Message
	{
		public const uint protocolId = 175;
		internal Boolean _isInitialized = false;
		public uint timestamp = 0;
		public int timezoneOffset = 0;
		
		public BasicTimeMessage()
		{
		}
		
		public BasicTimeMessage(uint arg1, int arg2)
			: this()
		{
			initBasicTimeMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 175;
		}
		
		public BasicTimeMessage initBasicTimeMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.timestamp = arg1;
			this.timezoneOffset = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.timestamp = 0;
			this.timezoneOffset = 0;
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
			this.serializeAs_BasicTimeMessage(arg1);
		}
		
		public void serializeAs_BasicTimeMessage(BigEndianWriter arg1)
		{
			if ( this.timestamp < 0 )
			{
				throw new Exception("Forbidden value (" + this.timestamp + ") on element timestamp.");
			}
			arg1.WriteInt((int)this.timestamp);
			arg1.WriteShort((short)this.timezoneOffset);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicTimeMessage(arg1);
		}
		
		public void deserializeAs_BasicTimeMessage(BigEndianReader arg1)
		{
			this.timestamp = (uint)arg1.ReadInt();
			if ( this.timestamp < 0 )
			{
				throw new Exception("Forbidden value (" + this.timestamp + ") on element of BasicTimeMessage.timestamp.");
			}
			this.timezoneOffset = (int)arg1.ReadShort();
		}
		
	}
}
