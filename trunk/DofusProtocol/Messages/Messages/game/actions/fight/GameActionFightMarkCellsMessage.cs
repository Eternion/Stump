using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightMarkCellsMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5540;
		internal Boolean _isInitialized = false;
		public GameActionMark mark;
		
		public GameActionFightMarkCellsMessage()
		{
			this.mark = new GameActionMark();
		}
		
		public GameActionFightMarkCellsMessage(uint arg1, int arg2, GameActionMark arg3)
			: this()
		{
			initGameActionFightMarkCellsMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5540;
		}
		
		public GameActionFightMarkCellsMessage initGameActionFightMarkCellsMessage(uint arg1 = 0, int arg2 = 0, GameActionMark arg3 = null)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.mark = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.mark = new GameActionMark();
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
			this.serializeAs_GameActionFightMarkCellsMessage(arg1);
		}
		
		public void serializeAs_GameActionFightMarkCellsMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			this.mark.serializeAs_GameActionMark(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightMarkCellsMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightMarkCellsMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.mark = new GameActionMark();
			this.mark.deserialize(arg1);
		}
		
	}
}
