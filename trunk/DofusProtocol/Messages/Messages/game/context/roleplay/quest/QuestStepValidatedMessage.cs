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
	
	public class QuestStepValidatedMessage : Message
	{
		public const uint protocolId = 6099;
		internal Boolean _isInitialized = false;
		public uint questId = 0;
		public uint stepId = 0;
		
		public QuestStepValidatedMessage()
		{
		}
		
		public QuestStepValidatedMessage(uint arg1, uint arg2)
			: this()
		{
			initQuestStepValidatedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6099;
		}
		
		public QuestStepValidatedMessage initQuestStepValidatedMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.questId = arg1;
			this.stepId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.questId = 0;
			this.stepId = 0;
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
			this.serializeAs_QuestStepValidatedMessage(arg1);
		}
		
		public void serializeAs_QuestStepValidatedMessage(BigEndianWriter arg1)
		{
			if ( this.questId < 0 || this.questId > 65535 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element questId.");
			}
			arg1.WriteShort((short)this.questId);
			if ( this.stepId < 0 || this.stepId > 65535 )
			{
				throw new Exception("Forbidden value (" + this.stepId + ") on element stepId.");
			}
			arg1.WriteShort((short)this.stepId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_QuestStepValidatedMessage(arg1);
		}
		
		public void deserializeAs_QuestStepValidatedMessage(BigEndianReader arg1)
		{
			this.questId = (uint)arg1.ReadUShort();
			if ( this.questId < 0 || this.questId > 65535 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element of QuestStepValidatedMessage.questId.");
			}
			this.stepId = (uint)arg1.ReadUShort();
			if ( this.stepId < 0 || this.stepId > 65535 )
			{
				throw new Exception("Forbidden value (" + this.stepId + ") on element of QuestStepValidatedMessage.stepId.");
			}
		}
		
	}
}
