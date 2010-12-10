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
	
	public class PaddockBuyableInformations : PaddockInformations
	{
		public const uint protocolId = 130;
		public uint price = 0;
		
		public PaddockBuyableInformations()
		{
		}
		
		public PaddockBuyableInformations(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initPaddockBuyableInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 130;
		}
		
		public PaddockBuyableInformations initPaddockBuyableInformations(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initPaddockInformations(arg1, arg2);
			this.price = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.price = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockBuyableInformations(arg1);
		}
		
		public void serializeAs_PaddockBuyableInformations(BigEndianWriter arg1)
		{
			base.serializeAs_PaddockInformations(arg1);
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockBuyableInformations(arg1);
		}
		
		public void deserializeAs_PaddockBuyableInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of PaddockBuyableInformations.price.");
			}
		}
		
	}
}
