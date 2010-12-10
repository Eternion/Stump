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
	
	public class SpellItem : Item
	{
		public const uint protocolId = 49;
		public uint position = 0;
		public int spellId = 0;
		public int spellLevel = 0;
		
		public SpellItem()
		{
		}
		
		public SpellItem(uint arg1, int arg2, int arg3)
			: this()
		{
			initSpellItem(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 49;
		}
		
		public SpellItem initSpellItem(uint arg1 = 0, int arg2 = 0, int arg3 = 0)
		{
			this.position = arg1;
			this.spellId = arg2;
			this.spellLevel = arg3;
			return this;
		}
		
		public override void reset()
		{
			this.position = 0;
			this.spellId = 0;
			this.spellLevel = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SpellItem(arg1);
		}
		
		public void serializeAs_SpellItem(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
			if ( this.position < 63 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element position.");
			}
			arg1.WriteByte((byte)this.position);
			arg1.WriteInt((int)this.spellId);
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element spellLevel.");
			}
			arg1.WriteByte((byte)this.spellLevel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellItem(arg1);
		}
		
		public void deserializeAs_SpellItem(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 63 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of SpellItem.position.");
			}
			this.spellId = (int)arg1.ReadInt();
			this.spellLevel = (int)arg1.ReadByte();
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element of SpellItem.spellLevel.");
			}
		}
		
	}
}
