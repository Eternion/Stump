using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterBaseInformations : CharacterMinimalPlusLookInformations
	{
		public const uint protocolId = 45;
		public int breed = 0;
		public Boolean sex = false;
		
		public CharacterBaseInformations()
		{
		}
		
		public CharacterBaseInformations(uint arg1, uint arg2, String arg3, EntityLook arg4, int arg5, Boolean arg6)
			: this()
		{
			initCharacterBaseInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 45;
		}
		
		public CharacterBaseInformations initCharacterBaseInformations(uint arg1 = 0, uint arg2 = 0, String arg3 = "", EntityLook arg4 = null, int arg5 = 0, Boolean arg6 = false)
		{
			base.initCharacterMinimalPlusLookInformations(arg1, arg2, arg3, arg4);
			this.breed = arg5;
			this.sex = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.breed = 0;
			this.sex = false;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterBaseInformations(arg1);
		}
		
		public void serializeAs_CharacterBaseInformations(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterMinimalPlusLookInformations(arg1);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteBoolean(this.sex);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterBaseInformations(arg1);
		}
		
		public void deserializeAs_CharacterBaseInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.breed = (int)arg1.ReadByte();
			this.sex = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
