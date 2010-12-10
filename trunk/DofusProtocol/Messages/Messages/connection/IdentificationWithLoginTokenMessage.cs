using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IdentificationWithLoginTokenMessage : IdentificationMessage
	{
		public const uint protocolId = 6201;
		internal Boolean _isInitialized = false;
		public String loginToken = "";
		
		public IdentificationWithLoginTokenMessage()
		{
		}
		
		public IdentificationWithLoginTokenMessage(Stump.DofusProtocol.Classes.Version arg1, String arg2, String arg3, Boolean arg4, String arg5)
			: this()
		{
			initIdentificationWithLoginTokenMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6201;
		}
		
		public IdentificationWithLoginTokenMessage initIdentificationWithLoginTokenMessage(Stump.DofusProtocol.Classes.Version arg1 = null, String arg2 = "", String arg3 = "", Boolean arg4 = false, String arg5 = "")
		{
			base.initIdentificationMessage(arg1, arg2, arg3, arg4);
			this.loginToken = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.loginToken = "";
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
			this.serializeAs_IdentificationWithLoginTokenMessage(arg1);
		}
		
		public void serializeAs_IdentificationWithLoginTokenMessage(BigEndianWriter arg1)
		{
			base.serializeAs_IdentificationMessage(arg1);
			arg1.WriteUTF((string)this.loginToken);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationWithLoginTokenMessage(arg1);
		}
		
		public void deserializeAs_IdentificationWithLoginTokenMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.loginToken = (String)arg1.ReadUTF();
		}
		
	}
}
