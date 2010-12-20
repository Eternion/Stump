using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightHumanReadyStateMessage : Message
	{
		public const uint protocolId = 740;
		internal Boolean _isInitialized = false;
		public uint characterId = 0;
		public Boolean isReady = false;
		
		public GameFightHumanReadyStateMessage()
		{
		}
		
		public GameFightHumanReadyStateMessage(uint arg1, Boolean arg2)
			: this()
		{
			initGameFightHumanReadyStateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 740;
		}
		
		public GameFightHumanReadyStateMessage initGameFightHumanReadyStateMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.characterId = arg1;
			this.isReady = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.characterId = 0;
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
			this.serializeAs_GameFightHumanReadyStateMessage(arg1);
		}
		
		public void serializeAs_GameFightHumanReadyStateMessage(BigEndianWriter arg1)
		{
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element characterId.");
			}
			arg1.WriteInt((int)this.characterId);
			arg1.WriteBoolean(this.isReady);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightHumanReadyStateMessage(arg1);
		}
		
		public void deserializeAs_GameFightHumanReadyStateMessage(BigEndianReader arg1)
		{
			this.characterId = (uint)arg1.ReadInt();
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element of GameFightHumanReadyStateMessage.characterId.");
			}
			this.isReady = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
