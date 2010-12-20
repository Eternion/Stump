using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightExchangePositionsMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5527;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public int casterCellId = 0;
		public int targetCellId = 0;
		
		public GameActionFightExchangePositionsMessage()
		{
		}
		
		public GameActionFightExchangePositionsMessage(uint arg1, int arg2, int arg3, int arg4, int arg5)
			: this()
		{
			initGameActionFightExchangePositionsMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5527;
		}
		
		public GameActionFightExchangePositionsMessage initGameActionFightExchangePositionsMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0, int arg5 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.casterCellId = arg4;
			this.targetCellId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
			this.casterCellId = 0;
			this.targetCellId = 0;
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
			this.serializeAs_GameActionFightExchangePositionsMessage(arg1);
		}
		
		public void serializeAs_GameActionFightExchangePositionsMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			if ( this.casterCellId < -1 || this.casterCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.casterCellId + ") on element casterCellId.");
			}
			arg1.WriteShort((short)this.casterCellId);
			if ( this.targetCellId < -1 || this.targetCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.targetCellId + ") on element targetCellId.");
			}
			arg1.WriteShort((short)this.targetCellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightExchangePositionsMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightExchangePositionsMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.casterCellId = (int)arg1.ReadShort();
			if ( this.casterCellId < -1 || this.casterCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.casterCellId + ") on element of GameActionFightExchangePositionsMessage.casterCellId.");
			}
			this.targetCellId = (int)arg1.ReadShort();
			if ( this.targetCellId < -1 || this.targetCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.targetCellId + ") on element of GameActionFightExchangePositionsMessage.targetCellId.");
			}
		}
		
	}
}
