using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightLifePointsVariationMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5598;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public int delta = 0;
		
		public GameActionFightLifePointsVariationMessage()
		{
		}
		
		public GameActionFightLifePointsVariationMessage(uint arg1, int arg2, int arg3, int arg4)
			: this()
		{
			initGameActionFightLifePointsVariationMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5598;
		}
		
		public GameActionFightLifePointsVariationMessage initGameActionFightLifePointsVariationMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.delta = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
			this.delta = 0;
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
			this.serializeAs_GameActionFightLifePointsVariationMessage(arg1);
		}
		
		public void serializeAs_GameActionFightLifePointsVariationMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			arg1.WriteShort((short)this.delta);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightLifePointsVariationMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightLifePointsVariationMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.delta = (int)arg1.ReadShort();
		}
		
	}
}
