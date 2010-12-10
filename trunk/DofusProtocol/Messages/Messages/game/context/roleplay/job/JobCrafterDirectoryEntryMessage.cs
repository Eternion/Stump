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
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobCrafterDirectoryEntryMessage : Message
	{
		public const uint protocolId = 6044;
		internal Boolean _isInitialized = false;
		public JobCrafterDirectoryEntryPlayerInfo playerInfo;
		public List<JobCrafterDirectoryEntryJobInfo> jobInfoList;
		public EntityLook playerLook;
		
		public JobCrafterDirectoryEntryMessage()
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
			this.jobInfoList = new List<JobCrafterDirectoryEntryJobInfo>();
			this.playerLook = new EntityLook();
		}
		
		public JobCrafterDirectoryEntryMessage(JobCrafterDirectoryEntryPlayerInfo arg1, List<JobCrafterDirectoryEntryJobInfo> arg2, EntityLook arg3)
			: this()
		{
			initJobCrafterDirectoryEntryMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6044;
		}
		
		public JobCrafterDirectoryEntryMessage initJobCrafterDirectoryEntryMessage(JobCrafterDirectoryEntryPlayerInfo arg1 = null, List<JobCrafterDirectoryEntryJobInfo> arg2 = null, EntityLook arg3 = null)
		{
			this.playerInfo = arg1;
			this.jobInfoList = arg2;
			this.playerLook = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
			this.playerLook = new EntityLook();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobCrafterDirectoryEntryMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryEntryMessage(BigEndianWriter arg1)
		{
			this.playerInfo.serializeAs_JobCrafterDirectoryEntryPlayerInfo(arg1);
			arg1.WriteShort((short)this.jobInfoList.Count);
			var loc1 = 0;
			while ( loc1 < this.jobInfoList.Count )
			{
				this.jobInfoList[loc1].serializeAs_JobCrafterDirectoryEntryJobInfo(arg1);
				++loc1;
			}
			this.playerLook.serializeAs_EntityLook(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryEntryMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryEntryMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
			this.playerInfo.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new JobCrafterDirectoryEntryJobInfo()) as JobCrafterDirectoryEntryJobInfo).deserialize(arg1);
				this.jobInfoList.Add((JobCrafterDirectoryEntryJobInfo)loc3);
				++loc2;
			}
			this.playerLook = new EntityLook();
			this.playerLook.deserialize(arg1);
		}
		
	}
}
