using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LifePointsRegenBeginMessage : Message
	{
		public const uint protocolId = 5684;
		internal Boolean _isInitialized = false;
		public uint regenRate = 0;
		
		public LifePointsRegenBeginMessage()
		{
		}
		
		public LifePointsRegenBeginMessage(uint arg1)
			: this()
		{
			initLifePointsRegenBeginMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5684;
		}
		
		public LifePointsRegenBeginMessage initLifePointsRegenBeginMessage(uint arg1 = 0)
		{
			this.regenRate = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.regenRate = 0;
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
			this.serializeAs_LifePointsRegenBeginMessage(arg1);
		}
		
		public void serializeAs_LifePointsRegenBeginMessage(BigEndianWriter arg1)
		{
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element regenRate.");
			}
			arg1.WriteByte((byte)this.regenRate);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LifePointsRegenBeginMessage(arg1);
		}
		
		public void deserializeAs_LifePointsRegenBeginMessage(BigEndianReader arg1)
		{
			this.regenRate = (uint)arg1.ReadByte();
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element of LifePointsRegenBeginMessage.regenRate.");
			}
		}
		
	}
}
