using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IdentificationAccountForceMessage : IdentificationMessage
	{
		public const uint protocolId = 6119;
		internal Boolean _isInitialized = false;
		public String forcedAccountLogin = "";
		
		public IdentificationAccountForceMessage()
		{
		}
		
		public IdentificationAccountForceMessage(Stump.DofusProtocol.Classes.Version arg1, String arg2, String arg3, Boolean arg4, String arg5)
			: this()
		{
			initIdentificationAccountForceMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6119;
		}
		
		public IdentificationAccountForceMessage initIdentificationAccountForceMessage(Stump.DofusProtocol.Classes.Version arg1 = null, String arg2 = "", String arg3 = "", Boolean arg4 = false, String arg5 = "")
		{
			base.initIdentificationMessage(arg1, arg2, arg3, arg4);
			this.forcedAccountLogin = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.forcedAccountLogin = "";
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
			this.serializeAs_IdentificationAccountForceMessage(arg1);
		}
		
		public void serializeAs_IdentificationAccountForceMessage(BigEndianWriter arg1)
		{
			base.serializeAs_IdentificationMessage(arg1);
			arg1.WriteUTF((string)this.forcedAccountLogin);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationAccountForceMessage(arg1);
		}
		
		public void deserializeAs_IdentificationAccountForceMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.forcedAccountLogin = (String)arg1.ReadUTF();
		}
		
	}
}
