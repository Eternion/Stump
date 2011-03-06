using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffectCreature : ObjectEffect
	{
		public const uint protocolId = 71;
		public uint monsterFamilyId = 0;
		
		public ObjectEffectCreature()
		{
		}
		
		public ObjectEffectCreature(uint arg1, uint arg2)
			: this()
		{
			initObjectEffectCreature(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 71;
		}
		
		public ObjectEffectCreature initObjectEffectCreature(uint arg1 = 0, uint arg2 = 0)
		{
			base.initObjectEffect(arg1);
			this.monsterFamilyId = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.monsterFamilyId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectCreature(arg1);
		}
		
		public void serializeAs_ObjectEffectCreature(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			if ( this.monsterFamilyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.monsterFamilyId + ") on element monsterFamilyId.");
			}
			arg1.WriteShort((short)this.monsterFamilyId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectCreature(arg1);
		}
		
		public void deserializeAs_ObjectEffectCreature(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.monsterFamilyId = (uint)arg1.ReadShort();
			if ( this.monsterFamilyId < 0 )
			{
				throw new Exception("Forbidden value (" + this.monsterFamilyId + ") on element of ObjectEffectCreature.monsterFamilyId.");
			}
		}
		
	}
}
