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
	
	public class GoldItem : Item
	{
		public const uint protocolId = 123;
		public uint sum = 0;
		
		public GoldItem()
		{
		}
		
		public GoldItem(uint arg1)
			: this()
		{
			initGoldItem(arg1);
		}
		
		public override uint getTypeId()
		{
			return 123;
		}
		
		public GoldItem initGoldItem(uint arg1 = 0)
		{
			this.sum = arg1;
			return this;
		}
		
		public override void reset()
		{
			this.sum = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GoldItem(arg1);
		}
		
		public void serializeAs_GoldItem(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
			if ( this.sum < 0 )
			{
				throw new Exception("Forbidden value (" + this.sum + ") on element sum.");
			}
			arg1.WriteInt((int)this.sum);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GoldItem(arg1);
		}
		
		public void deserializeAs_GoldItem(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.sum = (uint)arg1.ReadInt();
			if ( this.sum < 0 )
			{
				throw new Exception("Forbidden value (" + this.sum + ") on element of GoldItem.sum.");
			}
		}
		
	}
}
