using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightSpectateMessage : Message
	{
		public const uint protocolId = 6069;
		internal Boolean _isInitialized = false;
		public List<FightDispellableEffectExtendedInformations> effects;
		public List<GameActionMark> marks;
		public uint gameTurn = 0;
		
		public GameFightSpectateMessage()
		{
			this.effects = new List<FightDispellableEffectExtendedInformations>();
			this.marks = new List<GameActionMark>();
		}
		
		public GameFightSpectateMessage(List<FightDispellableEffectExtendedInformations> arg1, List<GameActionMark> arg2, uint arg3)
			: this()
		{
			initGameFightSpectateMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6069;
		}
		
		public GameFightSpectateMessage initGameFightSpectateMessage(List<FightDispellableEffectExtendedInformations> arg1, List<GameActionMark> arg2, uint arg3 = 0)
		{
			this.effects = arg1;
			this.marks = arg2;
			this.gameTurn = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.effects = new List<FightDispellableEffectExtendedInformations>();
			this.marks = new List<GameActionMark>();
			this.gameTurn = 0;
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
			this.serializeAs_GameFightSpectateMessage(arg1);
		}
		
		public void serializeAs_GameFightSpectateMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.effects.Count);
			var loc1 = 0;
			while ( loc1 < this.effects.Count )
			{
				arg1.WriteShort((short)this.effects[loc1].getTypeId());
				this.effects[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.marks.Count);
			var loc2 = 0;
			while ( loc2 < this.marks.Count )
			{
				this.marks[loc2].serializeAs_GameActionMark(arg1);
				++loc2;
			}
			if ( this.gameTurn < 0 )
			{
				throw new Exception("Forbidden value (" + this.gameTurn + ") on element gameTurn.");
			}
			arg1.WriteShort((short)this.gameTurn);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightSpectateMessage(arg1);
		}
		
		public void deserializeAs_GameFightSpectateMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			object loc6 = null;
			object loc7 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = (ushort)arg1.ReadUShort();
				(( loc6 = ProtocolTypeManager.GetInstance<FightDispellableEffectExtendedInformations>((uint)loc5)) as FightDispellableEffectExtendedInformations).deserialize(arg1);
				this.effects.Add((FightDispellableEffectExtendedInformations)loc6);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				((loc7 = new GameActionMark()) as GameActionMark).deserialize(arg1);
				this.marks.Add((GameActionMark)loc7);
				++loc4;
			}
			this.gameTurn = (uint)arg1.ReadShort();
			if ( this.gameTurn < 0 )
			{
				throw new Exception("Forbidden value (" + this.gameTurn + ") on element of GameFightSpectateMessage.gameTurn.");
			}
		}
		
	}
}
