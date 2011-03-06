using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterMinimalPlusLookAndGradeInformations : CharacterMinimalPlusLookInformations
	{
		public const uint protocolId = 193;
		public uint grade = 0;
		
		public CharacterMinimalPlusLookAndGradeInformations()
		{
		}
		
		public CharacterMinimalPlusLookAndGradeInformations(uint arg1, uint arg2, String arg3, EntityLook arg4, uint arg5)
			: this()
		{
			initCharacterMinimalPlusLookAndGradeInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 193;
		}
		
		public CharacterMinimalPlusLookAndGradeInformations initCharacterMinimalPlusLookAndGradeInformations(uint arg1 = 0, uint arg2 = 0, String arg3 = "", EntityLook arg4 = null, uint arg5 = 0)
		{
			base.initCharacterMinimalPlusLookInformations(arg1, arg2, arg3, arg4);
			this.grade = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.grade = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterMinimalPlusLookAndGradeInformations(arg1);
		}
		
		public void serializeAs_CharacterMinimalPlusLookAndGradeInformations(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterMinimalPlusLookInformations(arg1);
			if ( this.grade < 0 )
			{
				throw new Exception("Forbidden value (" + this.grade + ") on element grade.");
			}
			arg1.WriteInt((int)this.grade);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterMinimalPlusLookAndGradeInformations(arg1);
		}
		
		public void deserializeAs_CharacterMinimalPlusLookAndGradeInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.grade = (uint)arg1.ReadInt();
			if ( this.grade < 0 )
			{
				throw new Exception("Forbidden value (" + this.grade + ") on element of CharacterMinimalPlusLookAndGradeInformations.grade.");
			}
		}
		
	}
}
