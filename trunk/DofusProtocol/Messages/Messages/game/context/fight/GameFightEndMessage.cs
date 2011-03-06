using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightEndMessage : Message
	{
		public const uint protocolId = 720;
		internal Boolean _isInitialized = false;
		public uint duration = 0;
		public int ageBonus = 0;
		public List<FightResultListEntry> results;
		
		public GameFightEndMessage()
		{
			this.results = new List<FightResultListEntry>();
		}
		
		public GameFightEndMessage(uint arg1, int arg2, List<FightResultListEntry> arg3)
			: this()
		{
			initGameFightEndMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 720;
		}
		
		public GameFightEndMessage initGameFightEndMessage(uint arg1 = 0, int arg2 = 0, List<FightResultListEntry> arg3 = null)
		{
			this.duration = arg1;
			this.ageBonus = arg2;
			this.results = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.duration = 0;
			this.ageBonus = 0;
			this.results = new List<FightResultListEntry>();
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
			this.serializeAs_GameFightEndMessage(arg1);
		}
		
		public void serializeAs_GameFightEndMessage(BigEndianWriter arg1)
		{
			if ( this.duration < 0 )
			{
				throw new Exception("Forbidden value (" + this.duration + ") on element duration.");
			}
			arg1.WriteInt((int)this.duration);
			arg1.WriteShort((short)this.ageBonus);
			arg1.WriteShort((short)this.results.Count);
			var loc1 = 0;
			while ( loc1 < this.results.Count )
			{
				arg1.WriteShort((short)this.results[loc1].getTypeId());
				this.results[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightEndMessage(arg1);
		}
		
		public void deserializeAs_GameFightEndMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			this.duration = (uint)arg1.ReadInt();
			if ( this.duration < 0 )
			{
				throw new Exception("Forbidden value (" + this.duration + ") on element of GameFightEndMessage.duration.");
			}
			this.ageBonus = (int)arg1.ReadShort();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<FightResultListEntry>((uint)loc3)) as FightResultListEntry).deserialize(arg1);
				this.results.Add((FightResultListEntry)loc4);
				++loc2;
			}
		}
		
	}
}
