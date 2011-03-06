using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightSummonMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5825;
		internal Boolean _isInitialized = false;
		public GameFightFighterInformations summon;
		
		public GameActionFightSummonMessage()
		{
			this.summon = new GameFightFighterInformations();
		}
		
		public GameActionFightSummonMessage(uint arg1, int arg2, GameFightFighterInformations arg3)
			: this()
		{
			initGameActionFightSummonMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5825;
		}
		
		public GameActionFightSummonMessage initGameActionFightSummonMessage(uint arg1 = 0, int arg2 = 0, GameFightFighterInformations arg3 = null)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.summon = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.summon = new GameFightFighterInformations();
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
			this.serializeAs_GameActionFightSummonMessage(arg1);
		}
		
		public void serializeAs_GameActionFightSummonMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteShort((short)this.summon.getTypeId());
			this.summon.serialize(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightSummonMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightSummonMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			this.summon = ProtocolTypeManager.GetInstance<GameFightFighterInformations>((uint)loc1);
			this.summon.deserialize(arg1);
		}
		
	}
}
