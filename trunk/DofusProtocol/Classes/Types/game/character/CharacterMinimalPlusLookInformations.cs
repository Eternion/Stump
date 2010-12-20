using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterMinimalPlusLookInformations : CharacterMinimalInformations
	{
		public const uint protocolId = 163;
		public EntityLook entityLook;
		
		public CharacterMinimalPlusLookInformations()
		{
			this.entityLook = new EntityLook();
		}
		
		public CharacterMinimalPlusLookInformations(uint arg1, uint arg2, String arg3, EntityLook arg4)
			: this()
		{
			initCharacterMinimalPlusLookInformations(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 163;
		}
		
		public CharacterMinimalPlusLookInformations initCharacterMinimalPlusLookInformations(uint arg1 = 0, uint arg2 = 0, String arg3 = "", EntityLook arg4 = null)
		{
			base.initCharacterMinimalInformations(arg1, arg2, arg3);
			this.entityLook = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.entityLook = new EntityLook();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterMinimalPlusLookInformations(arg1);
		}
		
		public void serializeAs_CharacterMinimalPlusLookInformations(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterMinimalInformations(arg1);
			this.entityLook.serializeAs_EntityLook(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterMinimalPlusLookInformations(arg1);
		}
		
		public void deserializeAs_CharacterMinimalPlusLookInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.entityLook = new EntityLook();
			this.entityLook.deserialize(arg1);
		}
		
	}
}
