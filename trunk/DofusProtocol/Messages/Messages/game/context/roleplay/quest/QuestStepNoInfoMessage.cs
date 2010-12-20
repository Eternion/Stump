using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class QuestStepNoInfoMessage : Message
	{
		public const uint protocolId = 5627;
		internal Boolean _isInitialized = false;
		public uint questId = 0;
		
		public QuestStepNoInfoMessage()
		{
		}
		
		public QuestStepNoInfoMessage(uint arg1)
			: this()
		{
			initQuestStepNoInfoMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5627;
		}
		
		public QuestStepNoInfoMessage initQuestStepNoInfoMessage(uint arg1 = 0)
		{
			this.questId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.questId = 0;
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
			this.serializeAs_QuestStepNoInfoMessage(arg1);
		}
		
		public void serializeAs_QuestStepNoInfoMessage(BigEndianWriter arg1)
		{
			if ( this.questId < 0 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element questId.");
			}
			arg1.WriteShort((short)this.questId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_QuestStepNoInfoMessage(arg1);
		}
		
		public void deserializeAs_QuestStepNoInfoMessage(BigEndianReader arg1)
		{
			this.questId = (uint)arg1.ReadShort();
			if ( this.questId < 0 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element of QuestStepNoInfoMessage.questId.");
			}
		}
		
	}
}
