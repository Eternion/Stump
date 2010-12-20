using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightTurnListMessage : Message
	{
		public const uint protocolId = 713;
		internal Boolean _isInitialized = false;
		public List<int> ids;
		public List<int> deadsIds;
		
		public GameFightTurnListMessage()
		{
			this.ids = new List<int>();
			this.deadsIds = new List<int>();
		}
		
		public GameFightTurnListMessage(List<int> arg1, List<int> arg2)
			: this()
		{
			initGameFightTurnListMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 713;
		}
		
		public GameFightTurnListMessage initGameFightTurnListMessage(List<int> arg1, List<int> arg2)
		{
			this.ids = arg1;
			this.deadsIds = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.ids = new List<int>();
			this.deadsIds = new List<int>();
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
			this.serializeAs_GameFightTurnListMessage(arg1);
		}
		
		public void serializeAs_GameFightTurnListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.ids.Count);
			var loc1 = 0;
			while ( loc1 < this.ids.Count )
			{
				arg1.WriteInt((int)this.ids[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.deadsIds.Count);
			var loc2 = 0;
			while ( loc2 < this.deadsIds.Count )
			{
				arg1.WriteInt((int)this.deadsIds[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightTurnListMessage(arg1);
		}
		
		public void deserializeAs_GameFightTurnListMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = arg1.ReadInt();
				this.ids.Add((int)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc6 = arg1.ReadInt();
				this.deadsIds.Add((int)loc6);
				++loc4;
			}
		}
		
	}
}
