using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightDispellableEffectExtendedInformations : Object
	{
		public const uint protocolId = 208;
		public uint actionId = 0;
		public int sourceId = 0;
		public AbstractFightDispellableEffect effect;
		
		public FightDispellableEffectExtendedInformations()
		{
			this.effect = new AbstractFightDispellableEffect();
		}
		
		public FightDispellableEffectExtendedInformations(uint arg1, int arg2, AbstractFightDispellableEffect arg3)
			: this()
		{
			initFightDispellableEffectExtendedInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 208;
		}
		
		public FightDispellableEffectExtendedInformations initFightDispellableEffectExtendedInformations(uint arg1 = 0, int arg2 = 0, AbstractFightDispellableEffect arg3 = null)
		{
			this.actionId = arg1;
			this.sourceId = arg2;
			this.effect = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.actionId = 0;
			this.sourceId = 0;
			this.effect = new AbstractFightDispellableEffect();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightDispellableEffectExtendedInformations(arg1);
		}
		
		public void serializeAs_FightDispellableEffectExtendedInformations(BigEndianWriter arg1)
		{
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteShort((short)this.actionId);
			arg1.WriteInt((int)this.sourceId);
			arg1.WriteShort((short)this.effect.getTypeId());
			this.effect.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightDispellableEffectExtendedInformations(arg1);
		}
		
		public void deserializeAs_FightDispellableEffectExtendedInformations(BigEndianReader arg1)
		{
			this.actionId = (uint)arg1.ReadShort();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of FightDispellableEffectExtendedInformations.actionId.");
			}
			this.sourceId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			this.effect = ProtocolTypeManager.GetInstance<AbstractFightDispellableEffect>((uint)loc1);
			this.effect.deserialize(arg1);
		}
		
	}
}
