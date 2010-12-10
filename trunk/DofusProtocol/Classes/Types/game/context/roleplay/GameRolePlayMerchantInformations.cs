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
	
	public class GameRolePlayMerchantInformations : GameRolePlayNamedActorInformations
	{
		public const uint protocolId = 129;
		public uint sellType = 0;
		
		public GameRolePlayMerchantInformations()
		{
		}
		
		public GameRolePlayMerchantInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4, uint arg5)
			: this()
		{
			initGameRolePlayMerchantInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 129;
		}
		
		public GameRolePlayMerchantInformations initGameRolePlayMerchantInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "", uint arg5 = 0)
		{
			base.initGameRolePlayNamedActorInformations(arg1, arg2, arg3, arg4);
			this.sellType = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.sellType = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayMerchantInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayMerchantInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayNamedActorInformations(arg1);
			if ( this.sellType < 0 )
			{
				throw new Exception("Forbidden value (" + this.sellType + ") on element sellType.");
			}
			arg1.WriteInt((int)this.sellType);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayMerchantInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayMerchantInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.sellType = (uint)arg1.ReadInt();
			if ( this.sellType < 0 )
			{
				throw new Exception("Forbidden value (" + this.sellType + ") on element of GameRolePlayMerchantInformations.sellType.");
			}
		}
		
	}
}
