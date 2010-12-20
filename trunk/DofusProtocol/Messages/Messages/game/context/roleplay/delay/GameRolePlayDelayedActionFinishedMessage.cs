using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayDelayedActionFinishedMessage : Message
	{
		public const uint protocolId = 6150;
		internal Boolean _isInitialized = false;
		public uint delayTypeId = 0;
		
		public GameRolePlayDelayedActionFinishedMessage()
		{
		}
		
		public GameRolePlayDelayedActionFinishedMessage(uint arg1)
			: this()
		{
			initGameRolePlayDelayedActionFinishedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6150;
		}
		
		public GameRolePlayDelayedActionFinishedMessage initGameRolePlayDelayedActionFinishedMessage(uint arg1 = 0)
		{
			this.delayTypeId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.delayTypeId = 0;
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
			this.serializeAs_GameRolePlayDelayedActionFinishedMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayDelayedActionFinishedMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.delayTypeId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayDelayedActionFinishedMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayDelayedActionFinishedMessage(BigEndianReader arg1)
		{
			this.delayTypeId = (uint)arg1.ReadByte();
			if ( this.delayTypeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.delayTypeId + ") on element of GameRolePlayDelayedActionFinishedMessage.delayTypeId.");
			}
		}
		
	}
}
