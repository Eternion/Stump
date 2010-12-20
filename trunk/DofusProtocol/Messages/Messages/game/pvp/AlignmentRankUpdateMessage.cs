using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AlignmentRankUpdateMessage : Message
	{
		public const uint protocolId = 6058;
		internal Boolean _isInitialized = false;
		public uint alignmentRank = 0;
		public Boolean verbose = false;
		
		public AlignmentRankUpdateMessage()
		{
		}
		
		public AlignmentRankUpdateMessage(uint arg1, Boolean arg2)
			: this()
		{
			initAlignmentRankUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6058;
		}
		
		public AlignmentRankUpdateMessage initAlignmentRankUpdateMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.alignmentRank = arg1;
			this.verbose = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.alignmentRank = 0;
			this.verbose = false;
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
			this.serializeAs_AlignmentRankUpdateMessage(arg1);
		}
		
		public void serializeAs_AlignmentRankUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.alignmentRank < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentRank + ") on element alignmentRank.");
			}
			arg1.WriteByte((byte)this.alignmentRank);
			arg1.WriteBoolean(this.verbose);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AlignmentRankUpdateMessage(arg1);
		}
		
		public void deserializeAs_AlignmentRankUpdateMessage(BigEndianReader arg1)
		{
			this.alignmentRank = (uint)arg1.ReadByte();
			if ( this.alignmentRank < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentRank + ") on element of AlignmentRankUpdateMessage.alignmentRank.");
			}
			this.verbose = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
