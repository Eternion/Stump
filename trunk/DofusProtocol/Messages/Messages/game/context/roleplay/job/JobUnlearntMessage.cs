using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobUnlearntMessage : Message
	{
		public const uint protocolId = 5657;
		internal Boolean _isInitialized = false;
		public uint jobId = 0;
		
		public JobUnlearntMessage()
		{
		}
		
		public JobUnlearntMessage(uint arg1)
			: this()
		{
			initJobUnlearntMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5657;
		}
		
		public JobUnlearntMessage initJobUnlearntMessage(uint arg1 = 0)
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
			this.serializeAs_JobUnlearntMessage(arg1);
		}
		
		public void serializeAs_JobUnlearntMessage(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobUnlearntMessage(arg1);
		}
		
		public void deserializeAs_JobUnlearntMessage(BigEndianReader arg1)
		{
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobUnlearntMessage.jobId.");
			}
		}
		
	}
}
