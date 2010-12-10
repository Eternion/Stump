using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IdentificationWithServerIdMessage : IdentificationMessage
	{
		public const uint protocolId = 6194;
		internal Boolean _isInitialized = false;
		public int serverId = 0;
		
		public IdentificationWithServerIdMessage()
		{
		}
		
		public IdentificationWithServerIdMessage(Stump.DofusProtocol.Classes.Version arg1, String arg2, String arg3, Boolean arg4, int arg5)
			: this()
		{
			initIdentificationWithServerIdMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6194;
		}
		
		public IdentificationWithServerIdMessage initIdentificationWithServerIdMessage(Stump.DofusProtocol.Classes.Version arg1 = null, String arg2 = "", String arg3 = "", Boolean arg4 = false, int arg5 = 0)
		{
			base.initIdentificationMessage(arg1, arg2, arg3, arg4);
			this.serverId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.serverId = 0;
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
			this.serializeAs_IdentificationWithServerIdMessage(arg1);
		}
		
		public void serializeAs_IdentificationWithServerIdMessage(BigEndianWriter arg1)
		{
			base.serializeAs_IdentificationMessage(arg1);
			arg1.WriteShort((short)this.serverId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationWithServerIdMessage(arg1);
		}
		
		public void deserializeAs_IdentificationWithServerIdMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.serverId = (int)arg1.ReadShort();
		}
		
	}
}
