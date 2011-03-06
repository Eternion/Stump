using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IgnoredDeleteResultMessage : Message
	{
		public const uint protocolId = 5677;
		internal Boolean _isInitialized = false;
		public Boolean success = false;
		public String name = "";
		public Boolean session = false;
		
		public IgnoredDeleteResultMessage()
		{
		}
		
		public IgnoredDeleteResultMessage(Boolean arg1, String arg2, Boolean arg3)
			: this()
		{
			initIgnoredDeleteResultMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5677;
		}
		
		public IgnoredDeleteResultMessage initIgnoredDeleteResultMessage(Boolean arg1 = false, String arg2 = "", Boolean arg3 = false)
		{
			this.success = arg1;
			this.name = arg2;
			this.session = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.success = false;
			this.name = "";
			this.session = false;
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
			this.serializeAs_IgnoredDeleteResultMessage(arg1);
		}
		
		public void serializeAs_IgnoredDeleteResultMessage(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.success);
			BooleanByteWrapper.SetFlag(loc1, 1, this.session);
			arg1.WriteByte((byte)loc1);
			arg1.WriteUTF((string)this.name);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IgnoredDeleteResultMessage(arg1);
		}
		
		public void deserializeAs_IgnoredDeleteResultMessage(BigEndianReader arg1)
		{
			var loc1 = arg1.ReadByte();
			this.success = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.session = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
