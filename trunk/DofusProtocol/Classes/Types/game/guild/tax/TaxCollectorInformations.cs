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
	
	public class TaxCollectorInformations : Object
	{
		public const uint protocolId = 167;
		public int uniqueId = 0;
		public uint firtNameId = 0;
		public uint lastNameId = 0;
		public AdditionalTaxCollectorInformations additonalInformation;
		public int worldX = 0;
		public int worldY = 0;
		public uint subAreaId = 0;
		public int state = 0;
		public EntityLook look;
		
		public TaxCollectorInformations()
		{
			this.additonalInformation = new AdditionalTaxCollectorInformations();
			this.look = new EntityLook();
		}
		
		public TaxCollectorInformations(int arg1, uint arg2, uint arg3, AdditionalTaxCollectorInformations arg4, int arg5, int arg6, uint arg7, int arg8, EntityLook arg9)
			: this()
		{
			initTaxCollectorInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public virtual uint getTypeId()
		{
			return 167;
		}
		
		public TaxCollectorInformations initTaxCollectorInformations(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, AdditionalTaxCollectorInformations arg4 = null, int arg5 = 0, int arg6 = 0, uint arg7 = 0, int arg8 = 0, EntityLook arg9 = null)
		{
			this.uniqueId = arg1;
			this.firtNameId = arg2;
			this.lastNameId = arg3;
			this.additonalInformation = arg4;
			this.worldX = arg5;
			this.worldY = arg6;
			this.subAreaId = arg7;
			this.state = arg8;
			this.look = arg9;
			return this;
		}
		
		public virtual void reset()
		{
			this.uniqueId = 0;
			this.firtNameId = 0;
			this.lastNameId = 0;
			this.additonalInformation = new AdditionalTaxCollectorInformations();
			this.worldY = 0;
			this.subAreaId = 0;
			this.state = 0;
			this.look = new EntityLook();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_TaxCollectorInformations(arg1);
		}
		
		public void serializeAs_TaxCollectorInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.uniqueId);
			if ( this.firtNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firtNameId + ") on element firtNameId.");
			}
			arg1.WriteShort((short)this.firtNameId);
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element lastNameId.");
			}
			arg1.WriteShort((short)this.lastNameId);
			this.additonalInformation.serializeAs_AdditionalTaxCollectorInformations(arg1);
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element subAreaId.");
			}
			arg1.WriteShort((short)this.subAreaId);
			arg1.WriteByte((byte)this.state);
			this.look.serializeAs_EntityLook(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorInformations(arg1);
		}
		
		public void deserializeAs_TaxCollectorInformations(BigEndianReader arg1)
		{
			this.uniqueId = (int)arg1.ReadInt();
			this.firtNameId = (uint)arg1.ReadShort();
			if ( this.firtNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firtNameId + ") on element of TaxCollectorInformations.firtNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of TaxCollectorInformations.lastNameId.");
			}
			this.additonalInformation = new AdditionalTaxCollectorInformations();
			this.additonalInformation.deserialize(arg1);
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of TaxCollectorInformations.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of TaxCollectorInformations.worldY.");
			}
			this.subAreaId = (uint)arg1.ReadShort();
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element of TaxCollectorInformations.subAreaId.");
			}
			this.state = (int)arg1.ReadByte();
			this.look = new EntityLook();
			this.look.deserialize(arg1);
		}
		
	}
}
