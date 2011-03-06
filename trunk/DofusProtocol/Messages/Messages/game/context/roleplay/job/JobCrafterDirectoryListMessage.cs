using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobCrafterDirectoryListMessage : Message
	{
		public const uint protocolId = 6046;
		internal Boolean _isInitialized = false;
		public List<JobCrafterDirectoryListEntry> listEntries;
		
		public JobCrafterDirectoryListMessage()
		{
			this.listEntries = new List<JobCrafterDirectoryListEntry>();
		}
		
		public JobCrafterDirectoryListMessage(List<JobCrafterDirectoryListEntry> arg1)
			: this()
		{
			initJobCrafterDirectoryListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6046;
		}
		
		public JobCrafterDirectoryListMessage initJobCrafterDirectoryListMessage(List<JobCrafterDirectoryListEntry> arg1)
		{
			this.listEntries = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.listEntries = new List<JobCrafterDirectoryListEntry>();
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
			this.serializeAs_JobCrafterDirectoryListMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.listEntries.Count);
			var loc1 = 0;
			while ( loc1 < this.listEntries.Count )
			{
				this.listEntries[loc1].serializeAs_JobCrafterDirectoryListEntry(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryListMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new JobCrafterDirectoryListEntry()) as JobCrafterDirectoryListEntry).deserialize(arg1);
				this.listEntries.Add((JobCrafterDirectoryListEntry)loc3);
				++loc2;
			}
		}
		
	}
}
