using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayDelayedActionMessage : Message
	{
		public const uint protocolId = 6153;
		internal Boolean _isInitialized = false;
		public uint delayTypeId = 0;
		public uint delayDuration = 0;
		
		public GameRolePlayDelayedActionMessage()
		{
		}
		
		public GameRolePlayDelayedActionMessage(uint arg1, uint arg2)
			: this()
		{
			initGameRolePlayDelayedActionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6153;
		}
		
		public GameRolePlayDelayedActionMessage initGameRolePlayDelayedActionMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.delayTypeId = arg1;
			this.delayDuration = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.delayTypeId = 0;
			this.delayDuration = 0;
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
			this.serializeAs_GameRolePlayDelayedActionMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayDelayedActionMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.delayTypeId);
			if ( this.delayDuration < 0 )
			{
				throw new Exception("Forbidden value (" + this.delayDuration + ") on element delayDuration.");
			}
			arg1.WriteInt((int)this.delayDuration);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayDelayedActionMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayDelayedActionMessage(BigEndianReader arg1)
		{
			this.delayTypeId = (uint)arg1.ReadByte();
			if ( this.delayTypeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.delayTypeId + ") on element of GameRolePlayDelayedActionMessage.delayTypeId.");
			}
			this.delayDuration = (uint)arg1.ReadInt();
			if ( this.delayDuration < 0 )
			{
				throw new Exception("Forbidden value (" + this.delayDuration + ") on element of GameRolePlayDelayedActionMessage.delayDuration.");
			}
		}
		
	}
}
