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
	
	public class TaxCollectorBasicInformations : Object
	{
		public const uint protocolId = 96;
		public uint firstNameId = 0;
		public uint lastNameId = 0;
		public int mapId = 0;
		
		public TaxCollectorBasicInformations()
		{
		}
		
		public TaxCollectorBasicInformations(uint arg1, uint arg2, int arg3)
			: this()
		{
			initTaxCollectorBasicInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 96;
		}
		
		public TaxCollectorBasicInformations initTaxCollectorBasicInformations(uint arg1 = 0, uint arg2 = 0, int arg3 = 0)
		{
			this.firstNameId = arg1;
			this.lastNameId = arg2;
			this.mapId = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.firstNameId = 0;
			this.lastNameId = 0;
			this.mapId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_TaxCollectorBasicInformations(arg1);
		}
		
		public void serializeAs_TaxCollectorBasicInformations(BigEndianWriter arg1)
		{
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element firstNameId.");
			}
			arg1.WriteShort((short)this.firstNameId);
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element lastNameId.");
			}
			arg1.WriteShort((short)this.lastNameId);
			arg1.WriteInt((int)this.mapId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorBasicInformations(arg1);
		}
		
		public void deserializeAs_TaxCollectorBasicInformations(BigEndianReader arg1)
		{
			this.firstNameId = (uint)arg1.ReadShort();
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element of TaxCollectorBasicInformations.firstNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of TaxCollectorBasicInformations.lastNameId.");
			}
			this.mapId = (int)arg1.ReadInt();
		}
		
	}
}
