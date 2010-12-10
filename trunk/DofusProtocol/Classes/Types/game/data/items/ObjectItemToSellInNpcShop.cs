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
	
	public class ObjectItemToSellInNpcShop : ObjectItemMinimalInformation
	{
		public const uint protocolId = 352;
		public uint objectPrice = 0;
		public String buyCriterion = "";
		
		public ObjectItemToSellInNpcShop()
		{
		}
		
		public ObjectItemToSellInNpcShop(uint arg1, int arg2, Boolean arg3, List<ObjectEffect> arg4, uint arg5, String arg6)
			: this()
		{
			initObjectItemToSellInNpcShop(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 352;
		}
		
		public ObjectItemToSellInNpcShop initObjectItemToSellInNpcShop(uint arg1 = 0, int arg2 = 0, Boolean arg3 = false, List<ObjectEffect> arg4 = null, uint arg5 = 0, String arg6 = "")
		{
			base.initObjectItemMinimalInformation(arg1, arg2, arg3, arg4);
			this.@objectPrice = arg5;
			this.buyCriterion = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.@objectPrice = 0;
			this.buyCriterion = "";
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectItemToSellInNpcShop(arg1);
		}
		
		public void serializeAs_ObjectItemToSellInNpcShop(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectItemMinimalInformation(arg1);
			if ( this.@objectPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectPrice + ") on element objectPrice.");
			}
			arg1.WriteInt((int)this.@objectPrice);
			arg1.WriteUTF((string)this.buyCriterion);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectItemToSellInNpcShop(arg1);
		}
		
		public void deserializeAs_ObjectItemToSellInNpcShop(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.@objectPrice = (uint)arg1.ReadInt();
			if ( this.@objectPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectPrice + ") on element of ObjectItemToSellInNpcShop.objectPrice.");
			}
			this.buyCriterion = (String)arg1.ReadUTF();
		}
		
	}
}
