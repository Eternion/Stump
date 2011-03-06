using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IdentificationSuccessWithLoginTokenMessage : IdentificationSuccessMessage
	{
		public const uint protocolId = 6209;
		internal Boolean _isInitialized = false;
		public String loginToken = "";
		
		public IdentificationSuccessWithLoginTokenMessage()
		{
		}
		
		public IdentificationSuccessWithLoginTokenMessage(String arg1, uint arg2, uint arg3, Boolean arg4, String arg5, double arg6, Boolean arg7, String arg8)
			: this()
		{
			initIdentificationSuccessWithLoginTokenMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public override uint getMessageId()
		{
			return 6209;
		}
		
		public IdentificationSuccessWithLoginTokenMessage initIdentificationSuccessWithLoginTokenMessage(String arg1 = "", uint arg2 = 0, uint arg3 = 0, Boolean arg4 = false, String arg5 = "", double arg6 = 0, Boolean arg7 = false, String arg8 = "")
		{
			base.initIdentificationSuccessMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			this.loginToken = arg8;
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
			this.serializeAs_IdentificationSuccessWithLoginTokenMessage(arg1);
		}
		
		public void serializeAs_IdentificationSuccessWithLoginTokenMessage(BigEndianWriter arg1)
		{
			base.serializeAs_IdentificationSuccessMessage(arg1);
			arg1.WriteUTF((string)this.loginToken);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationSuccessWithLoginTokenMessage(arg1);
		}
		
		public void deserializeAs_IdentificationSuccessWithLoginTokenMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.loginToken = (String)arg1.ReadUTF();
		}
		
	}
}
