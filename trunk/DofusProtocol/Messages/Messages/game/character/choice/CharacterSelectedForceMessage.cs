using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterSelectedForceMessage : Message
	{
		public const uint protocolId = 6068;
		internal Boolean _isInitialized = false;
		public int id = 0;
		
		public CharacterSelectedForceMessage()
		{
		}
		
		public CharacterSelectedForceMessage(int arg1)
			: this()
		{
			initCharacterSelectedForceMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6068;
		}
		
		public CharacterSelectedForceMessage initCharacterSelectedForceMessage(int arg1 = 0)
		{
			this.id = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
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
			this.serializeAs_CharacterSelectedForceMessage(arg1);
		}
		
		public void serializeAs_CharacterSelectedForceMessage(BigEndianWriter arg1)
		{
			if ( this.id < 1 || this.id > 2147483647 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterSelectedForceMessage(arg1);
		}
		
		public void deserializeAs_CharacterSelectedForceMessage(BigEndianReader arg1)
		{
			this.id = (int)arg1.ReadInt();
			if ( this.id < 1 || this.id > 2147483647 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of CharacterSelectedForceMessage.id.");
			}
		}
		
	}
}
