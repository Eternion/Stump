using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class UpdateLifePointsMessage : Message
	{
		public const uint protocolId = 5658;
		internal Boolean _isInitialized = false;
		public uint lifePoints = 0;
		public uint maxLifePoints = 0;
		
		public UpdateLifePointsMessage()
		{
		}
		
		public UpdateLifePointsMessage(uint arg1, uint arg2)
			: this()
		{
			initUpdateLifePointsMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5658;
		}
		
		public UpdateLifePointsMessage initUpdateLifePointsMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.lifePoints = arg1;
			this.maxLifePoints = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.lifePoints = 0;
			this.maxLifePoints = 0;
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
			this.serializeAs_UpdateLifePointsMessage(arg1);
		}
		
		public void serializeAs_UpdateLifePointsMessage(BigEndianWriter arg1)
		{
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element lifePoints.");
			}
			arg1.WriteInt((int)this.lifePoints);
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element maxLifePoints.");
			}
			arg1.WriteInt((int)this.maxLifePoints);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_UpdateLifePointsMessage(arg1);
		}
		
		public void deserializeAs_UpdateLifePointsMessage(BigEndianReader arg1)
		{
			this.lifePoints = (uint)arg1.ReadInt();
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element of UpdateLifePointsMessage.lifePoints.");
			}
			this.maxLifePoints = (uint)arg1.ReadInt();
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element of UpdateLifePointsMessage.maxLifePoints.");
			}
		}
		
	}
}
