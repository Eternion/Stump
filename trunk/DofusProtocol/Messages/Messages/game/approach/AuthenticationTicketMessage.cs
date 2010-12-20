using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AuthenticationTicketMessage : Message
	{
		public const uint protocolId = 110;
		internal Boolean _isInitialized = false;
		public String lang = "";
		public String ticket = "";
		
		public AuthenticationTicketMessage()
		{
		}
		
		public AuthenticationTicketMessage(String arg1, String arg2)
			: this()
		{
			initAuthenticationTicketMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 110;
		}
		
		public AuthenticationTicketMessage initAuthenticationTicketMessage(String arg1 = "", String arg2 = "")
		{
			this.lang = arg1;
			this.ticket = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.lang = "";
			this.ticket = "";
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
			this.serializeAs_AuthenticationTicketMessage(arg1);
		}
		
		public void serializeAs_AuthenticationTicketMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.lang);
			arg1.WriteUTF((string)this.ticket);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AuthenticationTicketMessage(arg1);
		}
		
		public void deserializeAs_AuthenticationTicketMessage(BigEndianReader arg1)
		{
			this.lang = (String)arg1.ReadUTF();
			this.ticket = (String)arg1.ReadUTF();
		}
		
	}
}
