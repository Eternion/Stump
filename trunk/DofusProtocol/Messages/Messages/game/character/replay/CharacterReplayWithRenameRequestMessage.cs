using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterReplayWithRenameRequestMessage : CharacterReplayRequestMessage
	{
		public const uint protocolId = 6122;
		internal Boolean _isInitialized = false;
		public String name = "";
		
		public CharacterReplayWithRenameRequestMessage()
		{
		}
		
		public CharacterReplayWithRenameRequestMessage(uint arg1, String arg2)
			: this()
		{
			initCharacterReplayWithRenameRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6122;
		}
		
		public CharacterReplayWithRenameRequestMessage initCharacterReplayWithRenameRequestMessage(uint arg1 = 0, String arg2 = "")
		{
			base.initCharacterReplayRequestMessage(arg1);
			this.name = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.name = "";
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
			this.serializeAs_CharacterReplayWithRenameRequestMessage(arg1);
		}
		
		public void serializeAs_CharacterReplayWithRenameRequestMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterReplayRequestMessage(arg1);
			arg1.WriteUTF((string)this.name);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterReplayWithRenameRequestMessage(arg1);
		}
		
		public void deserializeAs_CharacterReplayWithRenameRequestMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
