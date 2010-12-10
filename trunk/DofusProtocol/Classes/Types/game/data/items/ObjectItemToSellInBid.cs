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
	
	public class ObjectItemToSellInBid : ObjectItemToSell
	{
		public const uint protocolId = 164;
		public uint unsoldDelay = 0;
		
		public ObjectItemToSellInBid()
		{
		}
		
		public ObjectItemToSellInBid(uint arg1, int arg2, Boolean arg3, List<ObjectEffect> arg4, uint arg5, uint arg6, uint arg7, uint arg8)
			: this()
		{
			initObjectItemToSellInBid(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public override uint getTypeId()
		{
			return 164;
		}
		
		public ObjectItemToSellInBid initObjectItemToSellInBid(uint arg1 = 0, int arg2 = 0, Boolean arg3 = false, List<ObjectEffect> arg4 = null, uint arg5 = 0, uint arg6 = 0, uint arg7 = 0, uint arg8 = 0)
		{
			base.initObjectItemToSell(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			this.unsoldDelay = arg8;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.unsoldDelay = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectItemToSellInBid(arg1);
		}
		
		public void serializeAs_ObjectItemToSellInBid(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectItemToSell(arg1);
			if ( this.unsoldDelay < 0 )
			{
				throw new Exception("Forbidden value (" + this.unsoldDelay + ") on element unsoldDelay.");
			}
			arg1.WriteShort((short)this.unsoldDelay);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectItemToSellInBid(arg1);
		}
		
		public void deserializeAs_ObjectItemToSellInBid(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.unsoldDelay = (uint)arg1.ReadShort();
			if ( this.unsoldDelay < 0 )
			{
				throw new Exception("Forbidden value (" + this.unsoldDelay + ") on element of ObjectItemToSellInBid.unsoldDelay.");
			}
		}
		
	}
}
