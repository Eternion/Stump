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
	
	public class PresetItem : Object
	{
		public const uint protocolId = 354;
		public uint position = 63;
		public uint objGid = 0;
		public uint objUid = 0;
		
		public PresetItem()
		{
		}
		
		public PresetItem(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initPresetItem(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 354;
		}
		
		public PresetItem initPresetItem(uint arg1 = 63, uint arg2 = 0, uint arg3 = 0)
		{
			this.position = arg1;
			this.objGid = arg2;
			this.objUid = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.position = 63;
			this.objGid = 0;
			this.objUid = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PresetItem(arg1);
		}
		
		public void serializeAs_PresetItem(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.position);
			if ( this.objGid < 0 )
			{
				throw new Exception("Forbidden value (" + this.objGid + ") on element objGid.");
			}
			arg1.WriteInt((int)this.objGid);
			if ( this.objUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.objUid + ") on element objUid.");
			}
			arg1.WriteInt((int)this.objUid);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PresetItem(arg1);
		}
		
		public void deserializeAs_PresetItem(BigEndianReader arg1)
		{
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 0 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of PresetItem.position.");
			}
			this.objGid = (uint)arg1.ReadInt();
			if ( this.objGid < 0 )
			{
				throw new Exception("Forbidden value (" + this.objGid + ") on element of PresetItem.objGid.");
			}
			this.objUid = (uint)arg1.ReadInt();
			if ( this.objUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.objUid + ") on element of PresetItem.objUid.");
			}
		}
		
	}
}
