using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class NicknameChoiceRequestMessage : Message
	{
		public const uint protocolId = 5639;
		internal Boolean _isInitialized = false;
		public String nickname = "";
		
		public NicknameChoiceRequestMessage()
		{
		}
		
		public NicknameChoiceRequestMessage(String arg1)
			: this()
		{
			initNicknameChoiceRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5639;
		}
		
		public NicknameChoiceRequestMessage initNicknameChoiceRequestMessage(String arg1 = "")
		{
			this.nickname = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nickname = "";
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
			this.serializeAs_NicknameChoiceRequestMessage(arg1);
		}
		
		public void serializeAs_NicknameChoiceRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.nickname);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NicknameChoiceRequestMessage(arg1);
		}
		
		public void deserializeAs_NicknameChoiceRequestMessage(BigEndianReader arg1)
		{
			this.nickname = (String)arg1.ReadUTF();
		}
		
	}
}
