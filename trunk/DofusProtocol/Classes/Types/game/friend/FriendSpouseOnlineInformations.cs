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
	
	public class FriendSpouseOnlineInformations : FriendSpouseInformations
	{
		public const uint protocolId = 93;
		public uint mapId = 0;
		public uint subAreaId = 0;
		public Boolean inFight = false;
		public Boolean followSpouse = false;
		public Boolean pvpEnabled = false;
		
		public FriendSpouseOnlineInformations()
		{
		}
		
		public FriendSpouseOnlineInformations(uint arg1, String arg2, uint arg3, int arg4, int arg5, EntityLook arg6, String arg7, int arg8, uint arg9, uint arg10, Boolean arg11, Boolean arg12, Boolean arg13)
			: this()
		{
			initFriendSpouseOnlineInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
		}
		
		public override uint getTypeId()
		{
			return 93;
		}
		
		public FriendSpouseOnlineInformations initFriendSpouseOnlineInformations(uint arg1 = 0, String arg2 = "", uint arg3 = 0, int arg4 = 0, int arg5 = 0, EntityLook arg6 = null, String arg7 = "", int arg8 = 0, uint arg9 = 0, uint arg10 = 0, Boolean arg11 = false, Boolean arg12 = false, Boolean arg13 = false)
		{
			base.initFriendSpouseInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
			this.mapId = arg9;
			this.subAreaId = arg10;
			this.inFight = arg11;
			this.followSpouse = arg12;
			this.pvpEnabled = arg13;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.mapId = 0;
			this.subAreaId = 0;
			this.inFight = false;
			this.followSpouse = false;
			this.pvpEnabled = false;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FriendSpouseOnlineInformations(arg1);
		}
		
		public void serializeAs_FriendSpouseOnlineInformations(BigEndianWriter arg1)
		{
			base.serializeAs_FriendSpouseInformations(arg1);
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.inFight);
			BooleanByteWrapper.SetFlag(loc1, 1, this.followSpouse);
			BooleanByteWrapper.SetFlag(loc1, 2, this.pvpEnabled);
			arg1.WriteByte((byte)loc1);
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element subAreaId.");
			}
			arg1.WriteShort((short)this.subAreaId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendSpouseOnlineInformations(arg1);
		}
		
		public void deserializeAs_FriendSpouseOnlineInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			var loc1 = arg1.ReadByte();
			this.inFight = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.followSpouse = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.pvpEnabled = (Boolean)BooleanByteWrapper.GetFlag(loc1, 2);
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of FriendSpouseOnlineInformations.mapId.");
			}
			this.subAreaId = (uint)arg1.ReadShort();
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element of FriendSpouseOnlineInformations.subAreaId.");
			}
		}
		
	}
}
