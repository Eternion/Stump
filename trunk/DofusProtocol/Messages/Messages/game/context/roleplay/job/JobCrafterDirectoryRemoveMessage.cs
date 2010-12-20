using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobCrafterDirectoryRemoveMessage : Message
	{
		public const uint protocolId = 5653;
		internal Boolean _isInitialized = false;
		public uint jobId = 0;
		public uint playerId = 0;
		
		public JobCrafterDirectoryRemoveMessage()
		{
		}
		
		public JobCrafterDirectoryRemoveMessage(uint arg1, uint arg2)
			: this()
		{
			initJobCrafterDirectoryRemoveMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5653;
		}
		
		public JobCrafterDirectoryRemoveMessage initJobCrafterDirectoryRemoveMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.jobId = arg1;
			this.playerId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.jobId = 0;
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
			this.serializeAs_JobCrafterDirectoryRemoveMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryRemoveMessage(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryRemoveMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryRemoveMessage(BigEndianReader arg1)
		{
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobCrafterDirectoryRemoveMessage.jobId.");
			}
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of JobCrafterDirectoryRemoveMessage.playerId.");
			}
		}
		
	}
}
