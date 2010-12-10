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
	
	public class PaddockContentInformations : PaddockInformations
	{
		public const uint protocolId = 183;
		public int mapId = 0;
		public List<MountInformationsForPaddock> mountsInformations;
		
		public PaddockContentInformations()
		{
			this.mountsInformations = new List<MountInformationsForPaddock>();
		}
		
		public PaddockContentInformations(uint arg1, uint arg2, int arg3, List<MountInformationsForPaddock> arg4)
			: this()
		{
			initPaddockContentInformations(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 183;
		}
		
		public PaddockContentInformations initPaddockContentInformations(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, List<MountInformationsForPaddock> arg4 = null)
		{
			base.initPaddockInformations(arg1, arg2);
			this.mapId = arg3;
			this.mountsInformations = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.mapId = 0;
			this.mountsInformations = new List<MountInformationsForPaddock>();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockContentInformations(arg1);
		}
		
		public void serializeAs_PaddockContentInformations(BigEndianWriter arg1)
		{
			base.serializeAs_PaddockInformations(arg1);
			arg1.WriteInt((int)this.mapId);
			arg1.WriteShort((short)this.mountsInformations.Count);
			var loc1 = 0;
			while ( loc1 < this.mountsInformations.Count )
			{
				this.mountsInformations[loc1].serializeAs_MountInformationsForPaddock(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockContentInformations(arg1);
		}
		
		public void deserializeAs_PaddockContentInformations(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			this.mapId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new MountInformationsForPaddock()) as MountInformationsForPaddock).deserialize(arg1);
				this.mountsInformations.Add((MountInformationsForPaddock)loc3);
				++loc2;
			}
		}
		
	}
}
