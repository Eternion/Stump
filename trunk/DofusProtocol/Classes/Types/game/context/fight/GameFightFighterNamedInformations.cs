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
	
	public class GameFightFighterNamedInformations : GameFightFighterInformations
	{
		public const uint protocolId = 158;
		public String name = "";
		
		public GameFightFighterNamedInformations()
		{
		}
		
		public GameFightFighterNamedInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6, String arg7)
			: this()
		{
			initGameFightFighterNamedInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 158;
		}
		
		public GameFightFighterNamedInformations initGameFightFighterNamedInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null, String arg7 = "")
		{
			base.initGameFightFighterInformations(arg1, arg2, arg3, arg4, arg5, arg6);
			this.name = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.name = "";
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightFighterNamedInformations(arg1);
		}
		
		public void serializeAs_GameFightFighterNamedInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightFighterInformations(arg1);
			arg1.WriteUTF((string)this.name);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightFighterNamedInformations(arg1);
		}
		
		public void deserializeAs_GameFightFighterNamedInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
