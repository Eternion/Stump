using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterBaseCharacteristic : Object
	{
		public const uint protocolId = 4;
		public int @base = 0;
		public int objectsAndMountBonus = 0;
		public int alignGiftBonus = 0;
		public int contextModif = 0;
		
		public CharacterBaseCharacteristic()
		{
		}
		
		public CharacterBaseCharacteristic(int arg1, int arg2, int arg3, int arg4)
			: this()
		{
			initCharacterBaseCharacteristic(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 4;
		}
		
		public CharacterBaseCharacteristic initCharacterBaseCharacteristic(int arg1 = 0, int arg2 = 0, int arg3 = 0, int arg4 = 0)
		{
			this.@base = arg1;
			this.@objectsAndMountBonus = arg2;
			this.alignGiftBonus = arg3;
			this.contextModif = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.@base = 0;
			this.@objectsAndMountBonus = 0;
			this.alignGiftBonus = 0;
			this.contextModif = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterBaseCharacteristic(arg1);
		}
		
		public void serializeAs_CharacterBaseCharacteristic(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@base);
			arg1.WriteShort((short)this.@objectsAndMountBonus);
			arg1.WriteShort((short)this.alignGiftBonus);
			arg1.WriteShort((short)this.contextModif);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterBaseCharacteristic(arg1);
		}
		
		public void deserializeAs_CharacterBaseCharacteristic(BigEndianReader arg1)
		{
			this.@base = (int)arg1.ReadShort();
			this.@objectsAndMountBonus = (int)arg1.ReadShort();
			this.alignGiftBonus = (int)arg1.ReadShort();
			this.contextModif = (int)arg1.ReadShort();
		}
		
	}
}
