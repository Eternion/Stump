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
	
	public class FightOptionsInformations : Object
	{
		public const uint protocolId = 20;
		public Boolean isSecret = false;
		public Boolean isRestrictedToPartyOnly = false;
		public Boolean isClosed = false;
		public Boolean isAskingForHelp = false;
		
		public FightOptionsInformations()
		{
		}
		
		public FightOptionsInformations(Boolean arg1, Boolean arg2, Boolean arg3, Boolean arg4)
			: this()
		{
			initFightOptionsInformations(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 20;
		}
		
		public FightOptionsInformations initFightOptionsInformations(Boolean arg1 = false, Boolean arg2 = false, Boolean arg3 = false, Boolean arg4 = false)
		{
			this.isSecret = arg1;
			this.isRestrictedToPartyOnly = arg2;
			this.isClosed = arg3;
			this.isAskingForHelp = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.isSecret = false;
			this.isRestrictedToPartyOnly = false;
			this.isClosed = false;
			this.isAskingForHelp = false;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightOptionsInformations(arg1);
		}
		
		public void serializeAs_FightOptionsInformations(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.isSecret);
			BooleanByteWrapper.SetFlag(loc1, 1, this.isRestrictedToPartyOnly);
			BooleanByteWrapper.SetFlag(loc1, 2, this.isClosed);
			BooleanByteWrapper.SetFlag(loc1, 3, this.isAskingForHelp);
			arg1.WriteByte((byte)loc1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightOptionsInformations(arg1);
		}
		
		public void deserializeAs_FightOptionsInformations(BigEndianReader arg1)
		{
			var loc1 = arg1.ReadByte();
			this.isSecret = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.isRestrictedToPartyOnly = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.isClosed = (Boolean)BooleanByteWrapper.GetFlag(loc1, 2);
			this.isAskingForHelp = (Boolean)BooleanByteWrapper.GetFlag(loc1, 3);
		}
		
	}
}
