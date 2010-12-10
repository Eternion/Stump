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
	
	public class FightTemporaryBoostEffect : AbstractFightDispellableEffect
	{
		public const uint protocolId = 209;
		public int delta = 0;
		
		public FightTemporaryBoostEffect()
		{
		}
		
		public FightTemporaryBoostEffect(uint arg1, int arg2, int arg3, uint arg4, uint arg5, int arg6)
			: this()
		{
			initFightTemporaryBoostEffect(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 209;
		}
		
		public FightTemporaryBoostEffect initFightTemporaryBoostEffect(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, uint arg5 = 0, int arg6 = 0)
		{
			base.initAbstractFightDispellableEffect(arg1, arg2, arg3, arg4, arg5);
			this.delta = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.delta = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTemporaryBoostEffect(arg1);
		}
		
		public void serializeAs_FightTemporaryBoostEffect(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractFightDispellableEffect(arg1);
			arg1.WriteShort((short)this.delta);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTemporaryBoostEffect(arg1);
		}
		
		public void deserializeAs_FightTemporaryBoostEffect(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.delta = (int)arg1.ReadShort();
		}
		
	}
}
