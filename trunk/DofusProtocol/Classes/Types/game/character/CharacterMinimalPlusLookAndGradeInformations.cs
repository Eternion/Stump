// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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
