using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectJobAddedMessage : Message
	{
		public const uint protocolId = 6014;
		internal Boolean _isInitialized = false;
		public uint jobId = 0;
		
		public ObjectJobAddedMessage()
		{
		}
		
		public ObjectJobAddedMessage(uint arg1)
			: this()
		{
			initObjectJobAddedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6014;
		}
		
		public ObjectJobAddedMessage initObjectJobAddedMessage(uint arg1 = 0)
		{
			this.jobId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.jobId = 0;
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
			this.serializeAs_ObjectJobAddedMessage(arg1);
		}
		
		public void serializeAs_ObjectJobAddedMessage(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectJobAddedMessage(arg1);
		}
		
		public void deserializeAs_ObjectJobAddedMessage(BigEndianReader arg1)
		{
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of ObjectJobAddedMessage.jobId.");
			}
		}
		
	}
}
