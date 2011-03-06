using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapNpcsQuestStatusUpdateMessage : Message
	{
		public const uint protocolId = 5642;
		internal Boolean _isInitialized = false;
		public int mapId = 0;
		public List<int> npcsIdsCanGiveQuest;
		public List<int> npcsIdsCannotGiveQuest;
		
		public MapNpcsQuestStatusUpdateMessage()
		{
			this.npcsIdsCanGiveQuest = new List<int>();
			this.npcsIdsCannotGiveQuest = new List<int>();
		}
		
		public MapNpcsQuestStatusUpdateMessage(int arg1, List<int> arg2, List<int> arg3)
			: this()
		{
			initMapNpcsQuestStatusUpdateMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5642;
		}
		
		public MapNpcsQuestStatusUpdateMessage initMapNpcsQuestStatusUpdateMessage(int arg1 = 0, List<int> arg2 = null, List<int> arg3 = null)
		{
			this.mapId = arg1;
			this.npcsIdsCanGiveQuest = arg2;
			this.npcsIdsCannotGiveQuest = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mapId = 0;
			this.npcsIdsCanGiveQuest = new List<int>();
			this.npcsIdsCannotGiveQuest = new List<int>();
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
			this.serializeAs_MapNpcsQuestStatusUpdateMessage(arg1);
		}
		
		public void serializeAs_MapNpcsQuestStatusUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.mapId);
			arg1.WriteShort((short)this.npcsIdsCanGiveQuest.Count);
			var loc1 = 0;
			while ( loc1 < this.npcsIdsCanGiveQuest.Count )
			{
				arg1.WriteInt((int)this.npcsIdsCanGiveQuest[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.npcsIdsCannotGiveQuest.Count);
			var loc2 = 0;
			while ( loc2 < this.npcsIdsCannotGiveQuest.Count )
			{
				arg1.WriteInt((int)this.npcsIdsCannotGiveQuest[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapNpcsQuestStatusUpdateMessage(arg1);
		}
		
		public void deserializeAs_MapNpcsQuestStatusUpdateMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			this.mapId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = arg1.ReadInt();
				this.npcsIdsCanGiveQuest.Add((int)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc6 = arg1.ReadInt();
				this.npcsIdsCannotGiveQuest.Add((int)loc6);
				++loc4;
			}
		}
		
	}
}
