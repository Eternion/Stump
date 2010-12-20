using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightDodgePointLossMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5828;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public uint amount = 0;
		
		public GameActionFightDodgePointLossMessage()
		{
		}
		
		public GameActionFightDodgePointLossMessage(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initGameActionFightDodgePointLossMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5828;
		}
		
		public GameActionFightDodgePointLossMessage initGameActionFightDodgePointLossMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.amount = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
			this.amount = 0;
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
			this.serializeAs_GameActionFightDodgePointLossMessage(arg1);
		}
		
		public void serializeAs_GameActionFightDodgePointLossMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			if ( this.amount < 0 )
			{
				throw new Exception("Forbidden value (" + this.amount + ") on element amount.");
			}
			arg1.WriteShort((short)this.amount);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightDodgePointLossMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightDodgePointLossMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.amount = (uint)arg1.ReadShort();
			if ( this.amount < 0 )
			{
				throw new Exception("Forbidden value (" + this.amount + ") on element of GameActionFightDodgePointLossMessage.amount.");
			}
		}
		
	}
}
