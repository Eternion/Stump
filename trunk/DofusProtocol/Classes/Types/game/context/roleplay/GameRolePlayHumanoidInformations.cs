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
	
	public class GameRolePlayHumanoidInformations : GameRolePlayNamedActorInformations
	{
		public const uint protocolId = 159;
		public HumanInformations humanoidInfo;
		
		public GameRolePlayHumanoidInformations()
		{
			this.humanoidInfo = new HumanInformations();
		}
		
		public GameRolePlayHumanoidInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4, HumanInformations arg5)
			: this()
		{
			initGameRolePlayHumanoidInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 159;
		}
		
		public GameRolePlayHumanoidInformations initGameRolePlayHumanoidInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "", HumanInformations arg5 = null)
		{
			base.initGameRolePlayNamedActorInformations(arg1, arg2, arg3, arg4);
			this.humanoidInfo = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.humanoidInfo = new HumanInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayHumanoidInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayHumanoidInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayNamedActorInformations(arg1);
			arg1.WriteShort((short)this.humanoidInfo.getTypeId());
			this.humanoidInfo.serialize(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayHumanoidInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayHumanoidInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			this.humanoidInfo = ProtocolTypeManager.GetInstance<HumanInformations>((uint)loc1);
			this.humanoidInfo.deserialize(arg1);
		}
		
	}
}
