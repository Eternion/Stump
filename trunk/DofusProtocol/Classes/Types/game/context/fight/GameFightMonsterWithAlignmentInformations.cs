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
	
	public class GameFightMonsterWithAlignmentInformations : GameFightMonsterInformations
	{
		public const uint protocolId = 203;
		public ActorAlignmentInformations alignmentInfos;
		
		public GameFightMonsterWithAlignmentInformations()
		{
			this.alignmentInfos = new ActorAlignmentInformations();
		}
		
		public GameFightMonsterWithAlignmentInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6, uint arg7, uint arg8, ActorAlignmentInformations arg9)
			: this()
		{
			initGameFightMonsterWithAlignmentInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public override uint getTypeId()
		{
			return 203;
		}
		
		public GameFightMonsterWithAlignmentInformations initGameFightMonsterWithAlignmentInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null, uint arg7 = 0, uint arg8 = 0, ActorAlignmentInformations arg9 = null)
		{
			base.initGameFightMonsterInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			this.alignmentInfos = arg9;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.alignmentInfos = new ActorAlignmentInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightMonsterWithAlignmentInformations(arg1);
		}
		
		public void serializeAs_GameFightMonsterWithAlignmentInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightMonsterInformations(arg1);
			this.alignmentInfos.serializeAs_ActorAlignmentInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightMonsterWithAlignmentInformations(arg1);
		}
		
		public void deserializeAs_GameFightMonsterWithAlignmentInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.alignmentInfos = new ActorAlignmentInformations();
			this.alignmentInfos.deserialize(arg1);
		}
		
	}
}
