using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterSelectedSuccessMessage : Message
	{
		public const uint protocolId = 153;
		internal Boolean _isInitialized = false;
		public CharacterBaseInformations infos;
		
		public CharacterSelectedSuccessMessage()
		{
			this.infos = new CharacterBaseInformations();
		}
		
		public CharacterSelectedSuccessMessage(CharacterBaseInformations arg1)
			: this()
		{
			initCharacterSelectedSuccessMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 153;
		}
		
		public CharacterSelectedSuccessMessage initCharacterSelectedSuccessMessage(CharacterBaseInformations arg1 = null)
		{
			this.infos = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.infos = new CharacterBaseInformations();
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
			this.serializeAs_CharacterSelectedSuccessMessage(arg1);
		}
		
		public void serializeAs_CharacterSelectedSuccessMessage(BigEndianWriter arg1)
		{
			this.infos.serializeAs_CharacterBaseInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterSelectedSuccessMessage(arg1);
		}
		
		public void deserializeAs_CharacterSelectedSuccessMessage(BigEndianReader arg1)
		{
			this.infos = new CharacterBaseInformations();
			this.infos.deserialize(arg1);
		}
		
	}
}
