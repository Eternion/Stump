using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightChangeLookMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5532;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public EntityLook entityLook;
		
		public GameActionFightChangeLookMessage()
		{
			this.entityLook = new EntityLook();
		}
		
		public GameActionFightChangeLookMessage(uint arg1, int arg2, int arg3, EntityLook arg4)
			: this()
		{
			initGameActionFightChangeLookMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5532;
		}
		
		public GameActionFightChangeLookMessage initGameActionFightChangeLookMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, EntityLook arg4 = null)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.entityLook = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
			this.entityLook = new EntityLook();
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
			this.serializeAs_GameActionFightChangeLookMessage(arg1);
		}
		
		public void serializeAs_GameActionFightChangeLookMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			this.entityLook.serializeAs_EntityLook(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightChangeLookMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightChangeLookMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.entityLook = new EntityLook();
			this.entityLook.deserialize(arg1);
		}
		
	}
}
