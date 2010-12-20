using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightTurnStartSlaveMessage : GameFightTurnStartMessage
	{
		public const uint protocolId = 6213;
		internal Boolean _isInitialized = false;
		public int idSummoner = 0;
		
		public GameFightTurnStartSlaveMessage()
		{
		}
		
		public GameFightTurnStartSlaveMessage(int arg1, uint arg2, int arg3)
			: this()
		{
			initGameFightTurnStartSlaveMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6213;
		}
		
		public GameFightTurnStartSlaveMessage initGameFightTurnStartSlaveMessage(int arg1 = 0, uint arg2 = 0, int arg3 = 0)
		{
			base.initGameFightTurnStartMessage(arg1, arg2);
			this.idSummoner = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.idSummoner = 0;
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
			this.serializeAs_GameFightTurnStartSlaveMessage(arg1);
		}
		
		public void serializeAs_GameFightTurnStartSlaveMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightTurnStartMessage(arg1);
			arg1.WriteInt((int)this.idSummoner);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightTurnStartSlaveMessage(arg1);
		}
		
		public void deserializeAs_GameFightTurnStartSlaveMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.idSummoner = (int)arg1.ReadInt();
		}
		
	}
}
