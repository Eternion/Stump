using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterNameSuggestionSuccessMessage : Message
	{
		public const uint protocolId = 5544;
		internal Boolean _isInitialized = false;
		public String suggestion = "";
		
		public CharacterNameSuggestionSuccessMessage()
		{
		}
		
		public CharacterNameSuggestionSuccessMessage(String arg1)
			: this()
		{
			initCharacterNameSuggestionSuccessMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5544;
		}
		
		public CharacterNameSuggestionSuccessMessage initCharacterNameSuggestionSuccessMessage(String arg1 = "")
		{
			this.suggestion = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.suggestion = "";
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
			this.serializeAs_CharacterNameSuggestionSuccessMessage(arg1);
		}
		
		public void serializeAs_CharacterNameSuggestionSuccessMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.suggestion);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterNameSuggestionSuccessMessage(arg1);
		}
		
		public void deserializeAs_CharacterNameSuggestionSuccessMessage(BigEndianReader arg1)
		{
			this.suggestion = (String)arg1.ReadUTF();
		}
		
	}
}
