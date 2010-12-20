using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class BasicPingMessage : Message
	{
		public const uint protocolId = 182;
		internal Boolean _isInitialized = false;
		public Boolean quiet = false;
		
		public BasicPingMessage()
		{
		}
		
		public BasicPingMessage(Boolean arg1)
			: this()
		{
			initBasicPingMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 182;
		}
		
		public BasicPingMessage initBasicPingMessage(Boolean arg1 = false)
		{
			this.quiet = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.quiet = false;
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
			this.serializeAs_BasicPingMessage(arg1);
		}
		
		public void serializeAs_BasicPingMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.quiet);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicPingMessage(arg1);
		}
		
		public void deserializeAs_BasicPingMessage(BigEndianReader arg1)
		{
			this.quiet = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
