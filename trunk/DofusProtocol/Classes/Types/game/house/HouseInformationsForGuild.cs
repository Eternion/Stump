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
	
	public class HouseInformationsForGuild : Object
	{
		public const uint protocolId = 170;
		public uint houseId = 0;
		public uint modelId = 0;
		public String ownerName = "";
		public int worldX = 0;
		public int worldY = 0;
		public List<int> skillListIds;
		public uint guildshareParams = 0;
		
		public HouseInformationsForGuild()
		{
			this.skillListIds = new List<int>();
		}
		
		public HouseInformationsForGuild(uint arg1, uint arg2, String arg3, int arg4, int arg5, List<int> arg6, uint arg7)
			: this()
		{
			initHouseInformationsForGuild(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public virtual uint getTypeId()
		{
			return 170;
		}
		
		public HouseInformationsForGuild initHouseInformationsForGuild(uint arg1 = 0, uint arg2 = 0, String arg3 = "", int arg4 = 0, int arg5 = 0, List<int> arg6 = null, uint arg7 = 0)
		{
			this.houseId = arg1;
			this.modelId = arg2;
			this.ownerName = arg3;
			this.worldX = arg4;
			this.worldY = arg5;
			this.skillListIds = arg6;
			this.guildshareParams = arg7;
			return this;
		}
		
		public virtual void reset()
		{
			this.houseId = 0;
			this.modelId = 0;
			this.ownerName = "";
			this.worldX = 0;
			this.worldY = 0;
			this.skillListIds = new List<int>();
			this.guildshareParams = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HouseInformationsForGuild(arg1);
		}
		
		public void serializeAs_HouseInformationsForGuild(BigEndianWriter arg1)
		{
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element houseId.");
			}
			arg1.WriteInt((int)this.houseId);
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element modelId.");
			}
			arg1.WriteInt((int)this.modelId);
			arg1.WriteUTF((string)this.ownerName);
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			arg1.WriteShort((short)this.skillListIds.Count);
			var loc1 = 0;
			while ( loc1 < this.skillListIds.Count )
			{
				arg1.WriteInt((int)this.skillListIds[loc1]);
				++loc1;
			}
			if ( this.guildshareParams < 0 || this.guildshareParams > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.guildshareParams + ") on element guildshareParams.");
			}
			arg1.WriteUInt((uint)this.guildshareParams);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseInformationsForGuild(arg1);
		}
		
		public void deserializeAs_HouseInformationsForGuild(BigEndianReader arg1)
		{
			var loc3 = 0;
			this.houseId = (uint)arg1.ReadInt();
			if ( this.houseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.houseId + ") on element of HouseInformationsForGuild.houseId.");
			}
			this.modelId = (uint)arg1.ReadInt();
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element of HouseInformationsForGuild.modelId.");
			}
			this.ownerName = (String)arg1.ReadUTF();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of HouseInformationsForGuild.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of HouseInformationsForGuild.worldY.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadInt();
				this.skillListIds.Add((int)loc3);
				++loc2;
			}
			this.guildshareParams = (uint)arg1.ReadUInt();
			if ( this.guildshareParams < 0 || this.guildshareParams > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.guildshareParams + ") on element of HouseInformationsForGuild.guildshareParams.");
			}
		}
		
	}
}
