using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StatsUpgradeResultMessage : Message
	{
		public const uint protocolId = 5609;
		internal Boolean _isInitialized = false;
		public uint nbCharacBoost = 0;
		
		public StatsUpgradeResultMessage()
		{
		}
		
		public StatsUpgradeResultMessage(uint arg1)
			: this()
		{
			initStatsUpgradeResultMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5609;
		}
		
		public StatsUpgradeResultMessage initStatsUpgradeResultMessage(uint arg1 = 0)
		{
			this.nbCharacBoost = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nbCharacBoost = 0;
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
			this.serializeAs_StatsUpgradeResultMessage(arg1);
		}
		
		public void serializeAs_StatsUpgradeResultMessage(BigEndianWriter arg1)
		{
			if ( this.nbCharacBoost < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbCharacBoost + ") on element nbCharacBoost.");
			}
			arg1.WriteShort((short)this.nbCharacBoost);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StatsUpgradeResultMessage(arg1);
		}
		
		public void deserializeAs_StatsUpgradeResultMessage(BigEndianReader arg1)
		{
			this.nbCharacBoost = (uint)arg1.ReadShort();
			if ( this.nbCharacBoost < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbCharacBoost + ") on element of StatsUpgradeResultMessage.nbCharacBoost.");
			}
		}
		
	}
}
