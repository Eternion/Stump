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
	
	public class AtlasPointsInformations : Object
	{
		public const uint protocolId = 175;
		public uint type = 0;
		public List<MapCoordinatesExtended> coords;
		
		public AtlasPointsInformations()
		{
			this.coords = new List<MapCoordinatesExtended>();
		}
		
		public AtlasPointsInformations(uint arg1, List<MapCoordinatesExtended> arg2)
			: this()
		{
			initAtlasPointsInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 175;
		}
		
		public AtlasPointsInformations initAtlasPointsInformations(uint arg1 = 0, List<MapCoordinatesExtended> arg2 = null)
		{
			this.type = arg1;
			this.coords = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.type = 0;
			this.coords = new List<MapCoordinatesExtended>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AtlasPointsInformations(arg1);
		}
		
		public void serializeAs_AtlasPointsInformations(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.type);
			arg1.WriteShort((short)this.coords.Count);
			var loc1 = 0;
			while ( loc1 < this.coords.Count )
			{
				this.coords[loc1].serializeAs_MapCoordinatesExtended(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AtlasPointsInformations(arg1);
		}
		
		public void deserializeAs_AtlasPointsInformations(BigEndianReader arg1)
		{
			object loc3 = null;
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of AtlasPointsInformations.type.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new MapCoordinatesExtended()) as MapCoordinatesExtended).deserialize(arg1);
				this.coords.Add((MapCoordinatesExtended)loc3);
				++loc2;
			}
		}
		
	}
}
