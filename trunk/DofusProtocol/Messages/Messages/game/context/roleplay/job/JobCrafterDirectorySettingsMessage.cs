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
	
	public class JobCrafterDirectorySettingsMessage : Message
	{
		public const uint protocolId = 5652;
		internal Boolean _isInitialized = false;
		public List<JobCrafterDirectorySettings> craftersSettings;
		
		public JobCrafterDirectorySettingsMessage()
		{
			this.craftersSettings = new List<JobCrafterDirectorySettings>();
		}
		
		public JobCrafterDirectorySettingsMessage(List<JobCrafterDirectorySettings> arg1)
			: this()
		{
			initJobCrafterDirectorySettingsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5652;
		}
		
		public JobCrafterDirectorySettingsMessage initJobCrafterDirectorySettingsMessage(List<JobCrafterDirectorySettings> arg1)
		{
			this.craftersSettings = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.craftersSettings = new List<JobCrafterDirectorySettings>();
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
			this.serializeAs_JobCrafterDirectorySettingsMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectorySettingsMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.craftersSettings.Count);
			var loc1 = 0;
			while ( loc1 < this.craftersSettings.Count )
			{
				this.craftersSettings[loc1].serializeAs_JobCrafterDirectorySettings(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectorySettingsMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectorySettingsMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new JobCrafterDirectorySettings()) as JobCrafterDirectorySettings).deserialize(arg1);
				this.craftersSettings.Add((JobCrafterDirectorySettings)loc3);
				++loc2;
			}
		}
		
	}
}
