using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class QuestValidatedMessage : Message
	{
		public const uint protocolId = 6097;
		internal Boolean _isInitialized = false;
		public uint questId = 0;
		
		public QuestValidatedMessage()
		{
		}
		
		public QuestValidatedMessage(uint arg1)
			: this()
		{
			initQuestValidatedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6097;
		}
		
		public QuestValidatedMessage initQuestValidatedMessage(uint arg1 = 0)
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
			this.serializeAs_QuestValidatedMessage(arg1);
		}
		
		public void serializeAs_QuestValidatedMessage(BigEndianWriter arg1)
		{
			if ( this.questId < 0 || this.questId > 65535 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element questId.");
			}
			arg1.WriteShort((short)this.questId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_QuestValidatedMessage(arg1);
		}
		
		public void deserializeAs_QuestValidatedMessage(BigEndianReader arg1)
		{
			this.questId = (uint)arg1.ReadUShort();
			if ( this.questId < 0 || this.questId > 65535 )
			{
				throw new Exception("Forbidden value (" + this.questId + ") on element of QuestValidatedMessage.questId.");
			}
		}
		
	}
}
