using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectErrorMessage : Message
	{
		public const uint protocolId = 3004;
		internal Boolean _isInitialized = false;
		public int reason = 0;
		
		public ObjectErrorMessage()
		{
		}
		
		public ObjectErrorMessage(int arg1)
			: this()
		{
			initObjectErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 3004;
		}
		
		public ObjectErrorMessage initObjectErrorMessage(int arg1 = 0)
		{
			this.reason = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.reason = 0;
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
			this.serializeAs_ObjectErrorMessage(arg1);
		}
		
		public void serializeAs_ObjectErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectErrorMessage(arg1);
		}
		
		public void deserializeAs_ObjectErrorMessage(BigEndianReader arg1)
		{
			this.reason = (int)arg1.ReadByte();
		}
		
	}
}
