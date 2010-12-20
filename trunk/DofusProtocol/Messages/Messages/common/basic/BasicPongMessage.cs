using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class BasicPongMessage : Message
	{
		public const uint protocolId = 183;
		internal Boolean _isInitialized = false;
		public Boolean quiet = false;
		
		public BasicPongMessage()
		{
		}
		
		public BasicPongMessage(Boolean arg1)
			: this()
		{
			initBasicPongMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 183;
		}
		
		public BasicPongMessage initBasicPongMessage(Boolean arg1 = false)
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
			this.serializeAs_BasicPongMessage(arg1);
		}
		
		public void serializeAs_BasicPongMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.quiet);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicPongMessage(arg1);
		}
		
		public void deserializeAs_BasicPongMessage(BigEndianReader arg1)
		{
			this.quiet = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
