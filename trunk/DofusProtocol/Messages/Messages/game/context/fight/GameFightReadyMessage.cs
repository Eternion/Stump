using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightReadyMessage : Message
	{
		public const uint protocolId = 708;
		internal Boolean _isInitialized = false;
		public Boolean isReady = false;
		
		public GameFightReadyMessage()
		{
		}
		
		public GameFightReadyMessage(Boolean arg1)
			: this()
		{
			initGameFightReadyMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 708;
		}
		
		public GameFightReadyMessage initGameFightReadyMessage(Boolean arg1 = false)
		{
			this.isReady = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.isReady = false;
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
			this.serializeAs_GameFightReadyMessage(arg1);
		}
		
		public void serializeAs_GameFightReadyMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.isReady);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightReadyMessage(arg1);
		}
		
		public void deserializeAs_GameFightReadyMessage(BigEndianReader arg1)
		{
			this.isReady = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
