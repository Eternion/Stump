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
	
	public class GameRolePlayMountInformations : GameRolePlayNamedActorInformations
	{
		public const uint protocolId = 180;
		public String ownerName = "";
		public uint level = 0;
		
		public GameRolePlayMountInformations()
		{
		}
		
		public GameRolePlayMountInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4, String arg5, uint arg6)
			: this()
		{
			initGameRolePlayMountInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 180;
		}
		
		public GameRolePlayMountInformations initGameRolePlayMountInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "", String arg5 = "", uint arg6 = 0)
		{
			base.initGameRolePlayNamedActorInformations(arg1, arg2, arg3, arg4);
			this.ownerName = arg5;
			this.level = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.ownerName = "";
			this.level = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayMountInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayMountInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayNamedActorInformations(arg1);
			arg1.WriteUTF((string)this.ownerName);
			if ( this.level < 0 || this.level > 255 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteByte((byte)this.level);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayMountInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayMountInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.ownerName = (String)arg1.ReadUTF();
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 0 || this.level > 255 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of GameRolePlayMountInformations.level.");
			}
		}
		
	}
}
