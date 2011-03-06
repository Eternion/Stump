using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StatsUpgradeRequestMessage : Message
	{
		public const uint protocolId = 5610;
		internal Boolean _isInitialized = false;
		public uint statId = 11;
		public uint boostPoint = 0;
		
		public StatsUpgradeRequestMessage()
		{
		}
		
		public StatsUpgradeRequestMessage(uint arg1, uint arg2)
			: this()
		{
			initStatsUpgradeRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5610;
		}
		
		public StatsUpgradeRequestMessage initStatsUpgradeRequestMessage(uint arg1 = 11, uint arg2 = 0)
		{
			this.statId = arg1;
			this.boostPoint = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.statId = 11;
			this.boostPoint = 0;
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
			this.serializeAs_StatsUpgradeRequestMessage(arg1);
		}
		
		public void serializeAs_StatsUpgradeRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.statId);
			if ( this.boostPoint < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoint + ") on element boostPoint.");
			}
			arg1.WriteShort((short)this.boostPoint);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StatsUpgradeRequestMessage(arg1);
		}
		
		public void deserializeAs_StatsUpgradeRequestMessage(BigEndianReader arg1)
		{
			this.statId = (uint)arg1.ReadByte();
			if ( this.statId < 0 )
			{
				throw new Exception("Forbidden value (" + this.statId + ") on element of StatsUpgradeRequestMessage.statId.");
			}
			this.boostPoint = (uint)arg1.ReadShort();
			if ( this.boostPoint < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoint + ") on element of StatsUpgradeRequestMessage.boostPoint.");
			}
		}
		
	}
}
