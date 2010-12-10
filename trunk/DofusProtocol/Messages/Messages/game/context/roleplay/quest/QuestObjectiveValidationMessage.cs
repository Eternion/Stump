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
	
	public class QuestObjectiveValidationMessage : Message
	{
		public const uint protocolId = 6085;
		internal Boolean _isInitialized = false;
		public uint questId = 0;
		public uint objectiveId = 0;
		
		public QuestObjectiveValidationMessage()
		{
		}
		
		public QuestObjectiveValidationMessage(uint arg1, uint arg2)
			: this()
		{
			initQuestObjectiveValidationMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6085;
		}
		
		public QuestObjectiveValidationMessage initQuestObjectiveValidationMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.questId = arg1;
			this.@objectiveId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.questId = 0;
			this.@objectiveId = 0;
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
			this.serializeAs_QuestObjectiveValidationMessage(arg1);
		}
		
		public void serializeAs_QuestObjectiveValidationMessage(BigEndianWriter arg1)
		{
			if ( this.questId < 0 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element questId.");
			}
			arg1.WriteShort((short)this.questId);
			if ( this.@objectiveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectiveId + ") on element objectiveId.");
			}
			arg1.WriteShort((short)this.@objectiveId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_QuestObjectiveValidationMessage(arg1);
		}
		
		public void deserializeAs_QuestObjectiveValidationMessage(BigEndianReader arg1)
		{
			this.questId = (uint)arg1.ReadShort();
			if ( this.questId < 0 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element of QuestObjectiveValidationMessage.questId.");
			}
			this.@objectiveId = (uint)arg1.ReadShort();
			if ( this.@objectiveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectiveId + ") on element of QuestObjectiveValidationMessage.objectiveId.");
			}
		}
		
	}
}
