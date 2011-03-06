using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class QuestListMessage : Message
	{
		public const uint protocolId = 5626;
		internal Boolean _isInitialized = false;
		public List<uint> finishedQuestsIds;
		public List<uint> activeQuestsIds;
		
		public QuestListMessage()
		{
			this.finishedQuestsIds = new List<uint>();
			this.activeQuestsIds = new List<uint>();
		}
		
		public QuestListMessage(List<uint> arg1, List<uint> arg2)
			: this()
		{
			initQuestListMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5626;
		}
		
		public QuestListMessage initQuestListMessage(List<uint> arg1, List<uint> arg2)
		{
			this.finishedQuestsIds = arg1;
			this.activeQuestsIds = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.finishedQuestsIds = new List<uint>();
			this.activeQuestsIds = new List<uint>();
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
			this.serializeAs_QuestListMessage(arg1);
		}
		
		public void serializeAs_QuestListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.finishedQuestsIds.Count);
			var loc1 = 0;
			while ( loc1 < this.finishedQuestsIds.Count )
			{
				if ( this.finishedQuestsIds[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.finishedQuestsIds[loc1] + ") on element 1 (starting at 1) of finishedQuestsIds.");
				}
				arg1.WriteShort((short)this.finishedQuestsIds[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.activeQuestsIds.Count);
			var loc2 = 0;
			while ( loc2 < this.activeQuestsIds.Count )
			{
				if ( this.activeQuestsIds[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.activeQuestsIds[loc2] + ") on element 2 (starting at 1) of activeQuestsIds.");
				}
				arg1.WriteShort((short)this.activeQuestsIds[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_QuestListMessage(arg1);
		}
		
		public void deserializeAs_QuestListMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of finishedQuestsIds.");
				}
				this.finishedQuestsIds.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of activeQuestsIds.");
				}
				this.activeQuestsIds.Add((uint)loc6);
				++loc4;
			}
		}
		
	}
}
