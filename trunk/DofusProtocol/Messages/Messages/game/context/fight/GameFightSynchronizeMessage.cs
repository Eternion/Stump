using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightSynchronizeMessage : Message
	{
		public const uint protocolId = 5921;
		internal Boolean _isInitialized = false;
		public List<GameFightFighterInformations> fighters;
		
		public GameFightSynchronizeMessage()
		{
			this.fighters = new List<GameFightFighterInformations>();
		}
		
		public GameFightSynchronizeMessage(List<GameFightFighterInformations> arg1)
			: this()
		{
			initGameFightSynchronizeMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5921;
		}
		
		public GameFightSynchronizeMessage initGameFightSynchronizeMessage(List<GameFightFighterInformations> arg1)
		{
			this.fighters = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fighters = new List<GameFightFighterInformations>();
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
			this.serializeAs_GameFightSynchronizeMessage(arg1);
		}
		
		public void serializeAs_GameFightSynchronizeMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.fighters.Count);
			var loc1 = 0;
			while ( loc1 < this.fighters.Count )
			{
				arg1.WriteShort((short)this.fighters[loc1].getTypeId());
				this.fighters[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightSynchronizeMessage(arg1);
		}
		
		public void deserializeAs_GameFightSynchronizeMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<GameFightFighterInformations>((uint)loc3)) as GameFightFighterInformations).deserialize(arg1);
				this.fighters.Add((GameFightFighterInformations)loc4);
				++loc2;
			}
		}
		
	}
}
