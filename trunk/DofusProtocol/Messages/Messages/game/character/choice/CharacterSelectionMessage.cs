using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterSelectionMessage : Message
	{
		public const uint protocolId = 152;
		internal Boolean _isInitialized = false;
		public int id = 0;
		
		public CharacterSelectionMessage()
		{
		}
		
		public CharacterSelectionMessage(int arg1)
			: this()
		{
			initCharacterSelectionMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 152;
		}
		
		public CharacterSelectionMessage initCharacterSelectionMessage(int arg1 = 0)
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
			this.serializeAs_CharacterSelectionMessage(arg1);
		}
		
		public void serializeAs_CharacterSelectionMessage(BigEndianWriter arg1)
		{
			if ( this.id < 1 || this.id > 2147483647 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterSelectionMessage(arg1);
		}
		
		public void deserializeAs_CharacterSelectionMessage(BigEndianReader arg1)
		{
			this.id = (int)arg1.ReadInt();
			if ( this.id < 1 || this.id > 2147483647 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of CharacterSelectionMessage.id.");
			}
		}
		
	}
}
