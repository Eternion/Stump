using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobCrafterDirectoryAddMessage : Message
	{
		public const uint protocolId = 5651;
		internal Boolean _isInitialized = false;
		public JobCrafterDirectoryListEntry listEntry;
		
		public JobCrafterDirectoryAddMessage()
		{
			this.listEntry = new JobCrafterDirectoryListEntry();
		}
		
		public JobCrafterDirectoryAddMessage(JobCrafterDirectoryListEntry arg1)
			: this()
		{
			initJobCrafterDirectoryAddMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5651;
		}
		
		public JobCrafterDirectoryAddMessage initJobCrafterDirectoryAddMessage(JobCrafterDirectoryListEntry arg1 = null)
		{
			this.listEntry = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.listEntry = new JobCrafterDirectoryListEntry();
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
			this.serializeAs_JobCrafterDirectoryAddMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryAddMessage(BigEndianWriter arg1)
		{
			this.listEntry.serializeAs_JobCrafterDirectoryListEntry(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryAddMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryAddMessage(BigEndianReader arg1)
		{
			this.listEntry = new JobCrafterDirectoryListEntry();
			this.listEntry.deserialize(arg1);
		}
		
	}
}
