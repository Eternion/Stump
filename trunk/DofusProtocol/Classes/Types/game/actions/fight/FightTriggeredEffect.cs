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
	
	public class FightTriggeredEffect : AbstractFightDispellableEffect
	{
		public const uint protocolId = 210;
		public int param1 = 0;
		public int param2 = 0;
		public int param3 = 0;
		public int delay = 0;
		
		public FightTriggeredEffect()
		{
		}
		
		public FightTriggeredEffect(uint arg1, int arg2, int arg3, uint arg4, uint arg5, int arg6, int arg7, int arg8, int arg9)
			: this()
		{
			initFightTriggeredEffect(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public override uint getTypeId()
		{
			return 210;
		}
		
		public FightTriggeredEffect initFightTriggeredEffect(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, uint arg5 = 0, int arg6 = 0, int arg7 = 0, int arg8 = 0, int arg9 = 0)
		{
			base.initAbstractFightDispellableEffect(arg1, arg2, arg3, arg4, arg5);
			this.param1 = arg6;
			this.param2 = arg7;
			this.param3 = arg8;
			this.delay = arg9;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.param1 = 0;
			this.param2 = 0;
			this.param3 = 0;
			this.delay = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTriggeredEffect(arg1);
		}
		
		public void serializeAs_FightTriggeredEffect(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractFightDispellableEffect(arg1);
			arg1.WriteInt((int)this.param1);
			arg1.WriteInt((int)this.param2);
			arg1.WriteInt((int)this.param3);
			arg1.WriteShort((short)this.delay);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTriggeredEffect(arg1);
		}
		
		public void deserializeAs_FightTriggeredEffect(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.param1 = (int)arg1.ReadInt();
			this.param2 = (int)arg1.ReadInt();
			this.param3 = (int)arg1.ReadInt();
			this.delay = (int)arg1.ReadShort();
		}
		
	}
}
