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
	
	public class FriendInformations : Object
	{
		public const uint protocolId = 78;
		public String name = "";
		public uint playerState = 99;
		public uint lastConnection = 0;
		
		public FriendInformations()
		{
		}
		
		public FriendInformations(String arg1, uint arg2, uint arg3)
			: this()
		{
			initFriendInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 78;
		}
		
		public FriendInformations initFriendInformations(String arg1 = "", uint arg2 = 99, uint arg3 = 0)
		{
			this.name = arg1;
			this.playerState = arg2;
			this.lastConnection = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.name = "";
			this.playerState = 99;
			this.lastConnection = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FriendInformations(arg1);
		}
		
		public void serializeAs_FriendInformations(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
			arg1.WriteByte((byte)this.playerState);
			if ( this.lastConnection < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastConnection + ") on element lastConnection.");
			}
			arg1.WriteInt((int)this.lastConnection);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendInformations(arg1);
		}
		
		public void deserializeAs_FriendInformations(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.playerState = (uint)arg1.ReadByte();
			if ( this.playerState < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerState + ") on element of FriendInformations.playerState.");
			}
			this.lastConnection = (uint)arg1.ReadInt();
			if ( this.lastConnection < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastConnection + ") on element of FriendInformations.lastConnection.");
			}
		}
		
	}
}
