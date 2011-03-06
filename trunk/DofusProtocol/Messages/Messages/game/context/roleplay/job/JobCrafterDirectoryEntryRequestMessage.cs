using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobCrafterDirectoryEntryRequestMessage : Message
	{
		public const uint protocolId = 6043;
		internal Boolean _isInitialized = false;
		public uint playerId = 0;
		
		public JobCrafterDirectoryEntryRequestMessage()
		{
		}
		
		public JobCrafterDirectoryEntryRequestMessage(uint arg1)
			: this()
		{
			initJobCrafterDirectoryEntryRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6043;
		}
		
		public JobCrafterDirectoryEntryRequestMessage initJobCrafterDirectoryEntryRequestMessage(uint arg1 = 0)
		{
			this.playerId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.playerId = 0;
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
			this.serializeAs_JobCrafterDirectoryEntryRequestMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryEntryRequestMessage(BigEndianWriter arg1)
		{
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryEntryRequestMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryEntryRequestMessage(BigEndianReader arg1)
		{
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of JobCrafterDirectoryEntryRequestMessage.playerId.");
			}
		}
		
	}
}
