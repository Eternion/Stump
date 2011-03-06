using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobListedUpdateMessage : Message
	{
		public const uint protocolId = 6016;
		internal Boolean _isInitialized = false;
		public Boolean addedOrDeleted = false;
		public uint jobId = 0;
		
		public JobListedUpdateMessage()
		{
		}
		
		public JobListedUpdateMessage(Boolean arg1, uint arg2)
			: this()
		{
			initJobListedUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6016;
		}
		
		public JobListedUpdateMessage initJobListedUpdateMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			this.addedOrDeleted = arg1;
			this.jobId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.addedOrDeleted = false;
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
			this.serializeAs_JobListedUpdateMessage(arg1);
		}
		
		public void serializeAs_JobListedUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.addedOrDeleted);
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobListedUpdateMessage(arg1);
		}
		
		public void deserializeAs_JobListedUpdateMessage(BigEndianReader arg1)
		{
			this.addedOrDeleted = (Boolean)arg1.ReadBoolean();
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobListedUpdateMessage.jobId.");
			}
		}
		
	}
}
