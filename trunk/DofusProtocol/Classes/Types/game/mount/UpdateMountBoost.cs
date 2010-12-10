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
	
	public class UpdateMountBoost : Object
	{
		public const uint protocolId = 356;
		public int type = 0;
		
		public UpdateMountBoost()
		{
		}
		
		public UpdateMountBoost(int arg1)
			: this()
		{
			initUpdateMountBoost(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 356;
		}
		
		public UpdateMountBoost initUpdateMountBoost(int arg1 = 0)
		{
			this.type = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.type = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_UpdateMountBoost(arg1);
		}
		
		public void serializeAs_UpdateMountBoost(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.type);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_UpdateMountBoost(arg1);
		}
		
		public void deserializeAs_UpdateMountBoost(BigEndianReader arg1)
		{
			this.type = (int)arg1.ReadByte();
		}
		
	}
}
