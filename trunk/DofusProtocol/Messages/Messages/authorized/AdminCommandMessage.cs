using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AdminCommandMessage : Message
	{
		public const uint protocolId = 76;
		internal Boolean _isInitialized = false;
		public String content = "";
		
		public AdminCommandMessage()
		{
		}
		
		public AdminCommandMessage(String arg1)
			: this()
		{
			initAdminCommandMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 76;
		}
		
		public AdminCommandMessage initAdminCommandMessage(String arg1 = "")
		{
			this.content = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.content = "";
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
			this.serializeAs_AdminCommandMessage(arg1);
		}
		
		public void serializeAs_AdminCommandMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.content);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AdminCommandMessage(arg1);
		}
		
		public void deserializeAs_AdminCommandMessage(BigEndianReader arg1)
		{
			this.content = (String)arg1.ReadUTF();
		}
		
	}
}
