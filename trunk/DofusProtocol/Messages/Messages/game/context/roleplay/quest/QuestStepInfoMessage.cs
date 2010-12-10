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
	
	public class QuestStepInfoMessage : Message
	{
		public const uint protocolId = 5625;
		internal Boolean _isInitialized = false;
		public uint questId = 0;
		public uint stepId = 0;
		public List<uint> objectivesIds;
		public List<Boolean> objectivesStatus;
		
		public QuestStepInfoMessage()
		{
			this.@objectivesIds = new List<uint>();
			this.@objectivesStatus = new List<Boolean>();
		}
		
		public QuestStepInfoMessage(uint arg1, uint arg2, List<uint> arg3, List<Boolean> arg4)
			: this()
		{
			initQuestStepInfoMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5625;
		}
		
		public QuestStepInfoMessage initQuestStepInfoMessage(uint arg1 = 0, uint arg2 = 0, List<uint> arg3 = null, List<Boolean> arg4 = null)
		{
			this.questId = arg1;
			this.stepId = arg2;
			this.@objectivesIds = arg3;
			this.@objectivesStatus = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.questId = 0;
			this.stepId = 0;
			this.@objectivesIds = new List<uint>();
			this.@objectivesStatus = new List<Boolean>();
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
			this.serializeAs_QuestStepInfoMessage(arg1);
		}
		
		public void serializeAs_QuestStepInfoMessage(BigEndianWriter arg1)
		{
			if ( this.questId < 0 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element questId.");
			}
			arg1.WriteShort((short)this.questId);
			if ( this.stepId < 0 )
			{
				throw new Exception("Forbidden value (" + this.stepId + ") on element stepId.");
			}
			arg1.WriteShort((short)this.stepId);
			arg1.WriteShort((short)this.@objectivesIds.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectivesIds.Count )
			{
				if ( this.@objectivesIds[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.@objectivesIds[loc1] + ") on element 3 (starting at 1) of objectivesIds.");
				}
				arg1.WriteShort((short)this.@objectivesIds[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.@objectivesStatus.Count);
			var loc2 = 0;
			while ( loc2 < this.@objectivesStatus.Count )
			{
				arg1.WriteBoolean(this.@objectivesStatus[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_QuestStepInfoMessage(arg1);
		}
		
		public void deserializeAs_QuestStepInfoMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = false;
			this.questId = (uint)arg1.ReadShort();
			if ( this.questId < 0 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element of QuestStepInfoMessage.questId.");
			}
			this.stepId = (uint)arg1.ReadShort();
			if ( this.stepId < 0 )
			{
				throw new Exception("Forbidden value (" + this.stepId + ") on element of QuestStepInfoMessage.stepId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of objectivesIds.");
				}
				this.@objectivesIds.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc6 = arg1.ReadBoolean();
				this.@objectivesStatus.Add((Boolean)loc6);
				++loc4;
			}
		}
		
	}
}
