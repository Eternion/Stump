using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightDispellableEffectMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 6070;
		internal Boolean _isInitialized = false;
		public AbstractFightDispellableEffect effect;
		
		public GameActionFightDispellableEffectMessage()
		{
			this.effect = new AbstractFightDispellableEffect();
		}
		
		public GameActionFightDispellableEffectMessage(uint arg1, int arg2, AbstractFightDispellableEffect arg3)
			: this()
		{
			initGameActionFightDispellableEffectMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6070;
		}
		
		public GameActionFightDispellableEffectMessage initGameActionFightDispellableEffectMessage(uint arg1 = 0, int arg2 = 0, AbstractFightDispellableEffect arg3 = null)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.effect = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.effect = new AbstractFightDispellableEffect();
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
			this.serializeAs_GameActionFightDispellableEffectMessage(arg1);
		}
		
		public void serializeAs_GameActionFightDispellableEffectMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteShort((short)this.effect.getTypeId());
			this.effect.serialize(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightDispellableEffectMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightDispellableEffectMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			this.effect = ProtocolTypeManager.GetInstance<AbstractFightDispellableEffect>((uint)loc1);
			this.effect.deserialize(arg1);
		}
		
	}
}
