using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffectLadder : ObjectEffectCreature
	{
		public const uint protocolId = 81;
		public uint monsterCount = 0;
		
		public ObjectEffectLadder()
		{
		}
		
		public ObjectEffectLadder(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initObjectEffectLadder(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 81;
		}
		
		public ObjectEffectLadder initObjectEffectLadder(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initObjectEffectCreature(arg1, arg2);
			this.monsterCount = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.monsterCount = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectLadder(arg1);
		}
		
		public void serializeAs_ObjectEffectLadder(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffectCreature(arg1);
			if ( this.monsterCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.monsterCount + ") on element monsterCount.");
			}
			arg1.WriteInt((int)this.monsterCount);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectLadder(arg1);
		}
		
		public void deserializeAs_ObjectEffectLadder(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.monsterCount = (uint)arg1.ReadInt();
			if ( this.monsterCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.monsterCount + ") on element of ObjectEffectLadder.monsterCount.");
			}
		}
		
	}
}
