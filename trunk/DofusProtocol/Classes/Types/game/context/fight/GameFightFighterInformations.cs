using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameFightFighterInformations : GameContextActorInformations
	{
		public const uint protocolId = 143;
		public uint teamId = 2;
		public Boolean alive = false;
		public GameFightMinimalStats stats;
		
		public GameFightFighterInformations()
		{
			this.stats = new GameFightMinimalStats();
		}
		
		public GameFightFighterInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6)
			: this()
		{
			initGameFightFighterInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 143;
		}
		
		public GameFightFighterInformations initGameFightFighterInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null)
		{
			base.initGameContextActorInformations(arg1, arg2, arg3);
			this.teamId = arg4;
			this.alive = arg5;
			this.stats = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.teamId = 2;
			this.alive = false;
			this.stats = new GameFightMinimalStats();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightFighterInformations(arg1);
		}
		
		public void serializeAs_GameFightFighterInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameContextActorInformations(arg1);
			arg1.WriteByte((byte)this.teamId);
			arg1.WriteBoolean(this.alive);
			arg1.WriteShort((short)this.stats.getTypeId());
			this.stats.serialize(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightFighterInformations(arg1);
		}
		
		public void deserializeAs_GameFightFighterInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.teamId = (uint)arg1.ReadByte();
			if ( this.teamId < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamId + ") on element of GameFightFighterInformations.teamId.");
			}
			this.alive = (Boolean)arg1.ReadBoolean();
			var loc1 = (ushort)arg1.ReadUShort();
			this.stats = ProtocolTypeManager.GetInstance<GameFightMinimalStats>((uint)loc1);
			this.stats.deserialize(arg1);
		}
		
	}
}
