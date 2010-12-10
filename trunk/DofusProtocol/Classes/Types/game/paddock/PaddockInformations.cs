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
	
	public class PaddockInformations : Object
	{
		public const uint protocolId = 132;
		public uint maxOutdoorMount = 0;
		public uint maxItems = 0;
		
		public PaddockInformations()
		{
		}
		
		public PaddockInformations(uint arg1, uint arg2)
			: this()
		{
			initPaddockInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 132;
		}
		
		public PaddockInformations initPaddockInformations(uint arg1 = 0, uint arg2 = 0)
		{
			this.maxOutdoorMount = arg1;
			this.maxItems = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.maxOutdoorMount = 0;
			this.maxItems = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockInformations(arg1);
		}
		
		public void serializeAs_PaddockInformations(BigEndianWriter arg1)
		{
			if ( this.maxOutdoorMount < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxOutdoorMount + ") on element maxOutdoorMount.");
			}
			arg1.WriteShort((short)this.maxOutdoorMount);
			if ( this.maxItems < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxItems + ") on element maxItems.");
			}
			arg1.WriteShort((short)this.maxItems);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockInformations(arg1);
		}
		
		public void deserializeAs_PaddockInformations(BigEndianReader arg1)
		{
			this.maxOutdoorMount = (uint)arg1.ReadShort();
			if ( this.maxOutdoorMount < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxOutdoorMount + ") on element of PaddockInformations.maxOutdoorMount.");
			}
			this.maxItems = (uint)arg1.ReadShort();
			if ( this.maxItems < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxItems + ") on element of PaddockInformations.maxItems.");
			}
		}
		
	}
}
