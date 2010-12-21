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
	
	public class BasicGuildInformations : Object
	{
		public const uint protocolId = 365;
		public uint guildId = 0;
		public String guildName = "";
		
		public BasicGuildInformations()
		{
		}
		
		public BasicGuildInformations(uint arg1, String arg2)
			: this()
		{
			initBasicGuildInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 365;
		}
		
		public BasicGuildInformations initBasicGuildInformations(uint arg1 = 0, String arg2 = "")
		{
			this.guildId = arg1;
			this.guildName = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.guildId = 0;
			this.guildName = "";
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_BasicGuildInformations(arg1);
		}
		
		public void serializeAs_BasicGuildInformations(BigEndianWriter arg1)
		{
			if ( this.guildId < 0 )
			{
				throw new Exception("Forbidden value (" + this.guildId + ") on element guildId.");
			}
			arg1.WriteInt((int)this.guildId);
			arg1.WriteUTF((string)this.guildName);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicGuildInformations(arg1);
		}
		
		public void deserializeAs_BasicGuildInformations(BigEndianReader arg1)
		{
			this.guildId = (uint)arg1.ReadInt();
			if ( this.guildId < 0 )
			{
				throw new Exception("Forbidden value (" + this.guildId + ") on element of BasicGuildInformations.guildId.");
			}
			this.guildName = (String)arg1.ReadUTF();
		}
		
	}
}
