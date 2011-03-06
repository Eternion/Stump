using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LifePointsRegenEndMessage : UpdateLifePointsMessage
	{
		public const uint protocolId = 5686;
		internal Boolean _isInitialized = false;
		public uint lifePointsGained = 0;
		
		public LifePointsRegenEndMessage()
		{
		}
		
		public LifePointsRegenEndMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initLifePointsRegenEndMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5686;
		}
		
		public LifePointsRegenEndMessage initLifePointsRegenEndMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initUpdateLifePointsMessage(arg1, arg2);
			this.lifePointsGained = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.lifePointsGained = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_LifePointsRegenEndMessage(arg1);
		}
		
		public void serializeAs_LifePointsRegenEndMessage(BigEndianWriter arg1)
		{
			base.serializeAs_UpdateLifePointsMessage(arg1);
			if ( this.lifePointsGained < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePointsGained + ") on element lifePointsGained.");
			}
			arg1.WriteInt((int)this.lifePointsGained);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LifePointsRegenEndMessage(arg1);
		}
		
		public void deserializeAs_LifePointsRegenEndMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.lifePointsGained = (uint)arg1.ReadInt();
			if ( this.lifePointsGained < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePointsGained + ") on element of LifePointsRegenEndMessage.lifePointsGained.");
			}
		}
		
	}
}
