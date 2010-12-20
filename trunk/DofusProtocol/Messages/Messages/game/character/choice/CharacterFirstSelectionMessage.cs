using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterFirstSelectionMessage : CharacterSelectionMessage
	{
		public const uint protocolId = 6084;
		internal Boolean _isInitialized = false;
		public Boolean doTutorial = false;
		
		public CharacterFirstSelectionMessage()
		{
		}
		
		public CharacterFirstSelectionMessage(int arg1, Boolean arg2)
			: this()
		{
			initCharacterFirstSelectionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6084;
		}
		
		public CharacterFirstSelectionMessage initCharacterFirstSelectionMessage(int arg1 = 0, Boolean arg2 = false)
		{
			base.initCharacterSelectionMessage(arg1);
			this.doTutorial = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.doTutorial = false;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterFirstSelectionMessage(arg1);
		}
		
		public void serializeAs_CharacterFirstSelectionMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterSelectionMessage(arg1);
			arg1.WriteBoolean(this.doTutorial);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterFirstSelectionMessage(arg1);
		}
		
		public void deserializeAs_CharacterFirstSelectionMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.doTutorial = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
