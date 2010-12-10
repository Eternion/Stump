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
	
	public class GameFightSpellCooldown : Object
	{
		public const uint protocolId = 205;
		public int spellId = 0;
		public uint cooldown = 0;
		
		public GameFightSpellCooldown()
		{
		}
		
		public GameFightSpellCooldown(int arg1, uint arg2)
			: this()
		{
			initGameFightSpellCooldown(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 205;
		}
		
		public GameFightSpellCooldown initGameFightSpellCooldown(int arg1 = 0, uint arg2 = 0)
		{
			this.spellId = arg1;
			this.cooldown = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.spellId = 0;
			this.cooldown = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightSpellCooldown(arg1);
		}
		
		public void serializeAs_GameFightSpellCooldown(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.spellId);
			if ( this.cooldown < 0 )
			{
				throw new Exception("Forbidden value (" + this.cooldown + ") on element cooldown.");
			}
			arg1.WriteByte((byte)this.cooldown);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightSpellCooldown(arg1);
		}
		
		public void deserializeAs_GameFightSpellCooldown(BigEndianReader arg1)
		{
			this.spellId = (int)arg1.ReadInt();
			this.cooldown = (uint)arg1.ReadByte();
			if ( this.cooldown < 0 )
			{
				throw new Exception("Forbidden value (" + this.cooldown + ") on element of GameFightSpellCooldown.cooldown.");
			}
		}
		
	}
}
