using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterReplayRequestMessage : Message
	{
		public const uint protocolId = 167;
		internal Boolean _isInitialized = false;
		public uint characterId = 0;
		
		public CharacterReplayRequestMessage()
		{
		}
		
		public CharacterReplayRequestMessage(uint arg1)
			: this()
		{
			initCharacterReplayRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 167;
		}
		
		public CharacterReplayRequestMessage initCharacterReplayRequestMessage(uint arg1 = 0)
		{
			this.characterId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.characterId = 0;
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
			this.serializeAs_CharacterReplayRequestMessage(arg1);
		}
		
		public void serializeAs_CharacterReplayRequestMessage(BigEndianWriter arg1)
		{
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element characterId.");
			}
			arg1.WriteInt((int)this.characterId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterReplayRequestMessage(arg1);
		}
		
		public void deserializeAs_CharacterReplayRequestMessage(BigEndianReader arg1)
		{
			this.characterId = (uint)arg1.ReadInt();
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element of CharacterReplayRequestMessage.characterId.");
			}
		}
		
	}
}
