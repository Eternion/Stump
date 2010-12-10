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
	
	public class JobCrafterDirectoryListEntry : Object
	{
		public const uint protocolId = 196;
		public JobCrafterDirectoryEntryPlayerInfo playerInfo;
		public JobCrafterDirectoryEntryJobInfo jobInfo;
		
		public JobCrafterDirectoryListEntry()
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
			this.jobInfo = new JobCrafterDirectoryEntryJobInfo();
		}
		
		public JobCrafterDirectoryListEntry(JobCrafterDirectoryEntryPlayerInfo arg1, JobCrafterDirectoryEntryJobInfo arg2)
			: this()
		{
			initJobCrafterDirectoryListEntry(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 196;
		}
		
		public JobCrafterDirectoryListEntry initJobCrafterDirectoryListEntry(JobCrafterDirectoryEntryPlayerInfo arg1 = null, JobCrafterDirectoryEntryJobInfo arg2 = null)
		{
			this.playerInfo = arg1;
			this.jobInfo = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobCrafterDirectoryListEntry(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryListEntry(BigEndianWriter arg1)
		{
			this.playerInfo.serializeAs_JobCrafterDirectoryEntryPlayerInfo(arg1);
			this.jobInfo.serializeAs_JobCrafterDirectoryEntryJobInfo(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryListEntry(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryListEntry(BigEndianReader arg1)
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
			this.playerInfo.deserialize(arg1);
			this.jobInfo = new JobCrafterDirectoryEntryJobInfo();
			this.jobInfo.deserialize(arg1);
		}
		
	}
}
