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
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SpellUpgradeSuccessMessage : Message
	{
		public const uint protocolId = 1201;
		internal Boolean _isInitialized = false;
		public int spellId = 0;
		public int spellLevel = 0;
		
		public SpellUpgradeSuccessMessage()
		{
		}
		
		public SpellUpgradeSuccessMessage(int arg1, int arg2)
			: this()
		{
			initSpellUpgradeSuccessMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1201;
		}
		
		public SpellUpgradeSuccessMessage initSpellUpgradeSuccessMessage(int arg1 = 0, int arg2 = 0)
		{
			this.spellId = arg1;
			this.spellLevel = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellId = 0;
			this.spellLevel = 0;
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SpellUpgradeSuccessMessage(arg1);
		}
		
		public void serializeAs_SpellUpgradeSuccessMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.spellId);
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element spellLevel.");
			}
			arg1.WriteByte((byte)this.spellLevel);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellUpgradeSuccessMessage(arg1);
		}
		
		public void deserializeAs_SpellUpgradeSuccessMessage(BigEndianReader arg1)
		{
			this.spellId = (int)arg1.ReadInt();
			this.spellLevel = (int)arg1.ReadByte();
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element of SpellUpgradeSuccessMessage.spellLevel.");
			}
		}
		
	}
}
