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
	
	public class ObjectEffectDice : ObjectEffect
	{
		public const uint protocolId = 73;
		public uint diceNum = 0;
		public uint diceSide = 0;
		public uint diceConst = 0;
		
		public ObjectEffectDice()
		{
		}
		
		public ObjectEffectDice(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initObjectEffectDice(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 73;
		}
		
		public ObjectEffectDice initObjectEffectDice(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			base.initObjectEffect(arg1);
			this.diceNum = arg2;
			this.diceSide = arg3;
			this.diceConst = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.diceNum = 0;
			this.diceSide = 0;
			this.diceConst = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectDice(arg1);
		}
		
		public void serializeAs_ObjectEffectDice(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			if ( this.diceNum < 0 )
			{
				throw new Exception("Forbidden value (" + this.diceNum + ") on element diceNum.");
			}
			arg1.WriteShort((short)this.diceNum);
			if ( this.diceSide < 0 )
			{
				throw new Exception("Forbidden value (" + this.diceSide + ") on element diceSide.");
			}
			arg1.WriteShort((short)this.diceSide);
			if ( this.diceConst < 0 )
			{
				throw new Exception("Forbidden value (" + this.diceConst + ") on element diceConst.");
			}
			arg1.WriteShort((short)this.diceConst);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectDice(arg1);
		}
		
		public void deserializeAs_ObjectEffectDice(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.diceNum = (uint)arg1.ReadShort();
			if ( this.diceNum < 0 )
			{
				throw new Exception("Forbidden value (" + this.diceNum + ") on element of ObjectEffectDice.diceNum.");
			}
			this.diceSide = (uint)arg1.ReadShort();
			if ( this.diceSide < 0 )
			{
				throw new Exception("Forbidden value (" + this.diceSide + ") on element of ObjectEffectDice.diceSide.");
			}
			this.diceConst = (uint)arg1.ReadShort();
			if ( this.diceConst < 0 )
			{
				throw new Exception("Forbidden value (" + this.diceConst + ") on element of ObjectEffectDice.diceConst.");
			}
		}
		
	}
}
