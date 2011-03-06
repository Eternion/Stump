using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterCreationResultMessage : Message
	{
		public const uint protocolId = 161;
		internal Boolean _isInitialized = false;
		public uint result = 1;
		
		public CharacterCreationResultMessage()
		{
		}
		
		public CharacterCreationResultMessage(uint arg1)
			: this()
		{
			initCharacterCreationResultMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 161;
		}
		
		public CharacterCreationResultMessage initCharacterCreationResultMessage(uint arg1 = 1)
		{
			this.result = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.result = 1;
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
			this.serializeAs_CharacterCreationResultMessage(arg1);
		}
		
		public void serializeAs_CharacterCreationResultMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.result);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterCreationResultMessage(arg1);
		}
		
		public void deserializeAs_CharacterCreationResultMessage(BigEndianReader arg1)
		{
			this.result = (uint)arg1.ReadByte();
			if ( this.result < 0 )
			{
				throw new Exception("Forbidden value (" + this.result + ") on element of CharacterCreationResultMessage.result.");
			}
		}
		
	}
}
