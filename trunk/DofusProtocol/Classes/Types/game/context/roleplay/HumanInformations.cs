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
	
	public class HumanInformations : Object
	{
		public const uint protocolId = 157;
		public List<EntityLook> followingCharactersLook;
		public int emoteId = 0;
		public uint emoteEndTime = 0;
		public ActorRestrictionsInformations restrictions;
		public uint titleId = 0;
		public String titleParam = "";
		
		public HumanInformations()
		{
			this.followingCharactersLook = new List<EntityLook>();
			this.restrictions = new ActorRestrictionsInformations();
		}
		
		public HumanInformations(List<EntityLook> arg1, int arg2, uint arg3, ActorRestrictionsInformations arg4, uint arg5, String arg6)
			: this()
		{
			initHumanInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public virtual uint getTypeId()
		{
			return 157;
		}
		
		public HumanInformations initHumanInformations(List<EntityLook> arg1, int arg2 = 0, uint arg3 = 0, ActorRestrictionsInformations arg4 = null, uint arg5 = 0, String arg6 = "")
		{
			this.followingCharactersLook = arg1;
			this.emoteId = arg2;
			this.emoteEndTime = arg3;
			this.restrictions = arg4;
			this.titleId = arg5;
			this.titleParam = arg6;
			return this;
		}
		
		public virtual void reset()
		{
			this.followingCharactersLook = new List<EntityLook>();
			this.emoteId = 0;
			this.emoteEndTime = 0;
			this.restrictions = new ActorRestrictionsInformations();
			this.titleParam = "";
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HumanInformations(arg1);
		}
		
		public void serializeAs_HumanInformations(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.followingCharactersLook.Count);
			var loc1 = 0;
			while ( loc1 < this.followingCharactersLook.Count )
			{
				this.followingCharactersLook[loc1].serializeAs_EntityLook(arg1);
				++loc1;
			}
			arg1.WriteByte((byte)this.emoteId);
			if ( this.emoteEndTime < 0 || this.emoteEndTime > 65535 )
			{
				throw new Exception("Forbidden value (" + this.emoteEndTime + ") on element emoteEndTime.");
			}
			arg1.WriteShort((short)this.emoteEndTime);
			this.restrictions.serializeAs_ActorRestrictionsInformations(arg1);
			if ( this.titleId < 0 )
			{
				throw new Exception("Forbidden value (" + this.titleId + ") on element titleId.");
			}
			arg1.WriteShort((short)this.titleId);
			arg1.WriteUTF((string)this.titleParam);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HumanInformations(arg1);
		}
		
		public void deserializeAs_HumanInformations(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new EntityLook()) as EntityLook).deserialize(arg1);
				this.followingCharactersLook.Add((EntityLook)loc3);
				++loc2;
			}
			this.emoteId = (int)arg1.ReadByte();
			this.emoteEndTime = (uint)arg1.ReadUShort();
			if ( this.emoteEndTime < 0 || this.emoteEndTime > 65535 )
			{
				throw new Exception("Forbidden value (" + this.emoteEndTime + ") on element of HumanInformations.emoteEndTime.");
			}
			this.restrictions = new ActorRestrictionsInformations();
			this.restrictions.deserialize(arg1);
			this.titleId = (uint)arg1.ReadShort();
			if ( this.titleId < 0 )
			{
				throw new Exception("Forbidden value (" + this.titleId + ") on element of HumanInformations.titleId.");
			}
			this.titleParam = (String)arg1.ReadUTF();
		}
		
	}
}
