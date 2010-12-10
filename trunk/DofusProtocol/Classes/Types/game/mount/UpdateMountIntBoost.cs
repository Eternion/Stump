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
	
	public class UpdateMountIntBoost : UpdateMountBoost
	{
		public const uint protocolId = 357;
		public int value = 0;
		
		public UpdateMountIntBoost()
		{
		}
		
		public UpdateMountIntBoost(int arg1, int arg2)
			: this()
		{
			initUpdateMountIntBoost(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 357;
		}
		
		public UpdateMountIntBoost initUpdateMountIntBoost(int arg1 = 0, int arg2 = 0)
		{
			base.initUpdateMountBoost(arg1);
			this.value = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.value = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_UpdateMountIntBoost(arg1);
		}
		
		public void serializeAs_UpdateMountIntBoost(BigEndianWriter arg1)
		{
			base.serializeAs_UpdateMountBoost(arg1);
			arg1.WriteInt((int)this.value);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_UpdateMountIntBoost(arg1);
		}
		
		public void deserializeAs_UpdateMountIntBoost(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.value = (int)arg1.ReadInt();
		}
		
	}
}
