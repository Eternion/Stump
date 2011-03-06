using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobCrafterDirectoryListRequestMessage : Message
	{
		public const uint protocolId = 6047;
		internal Boolean _isInitialized = false;
		public uint jobId = 0;
		
		public JobCrafterDirectoryListRequestMessage()
		{
		}
		
		public JobCrafterDirectoryListRequestMessage(uint arg1)
			: this()
		{
			initJobCrafterDirectoryListRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6047;
		}
		
		public JobCrafterDirectoryListRequestMessage initJobCrafterDirectoryListRequestMessage(uint arg1 = 0)
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
			this.serializeAs_JobCrafterDirectoryListRequestMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryListRequestMessage(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryListRequestMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryListRequestMessage(BigEndianReader arg1)
		{
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobCrafterDirectoryListRequestMessage.jobId.");
			}
		}
		
	}
}
