using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightResumeWithSlavesMessage : GameFightResumeMessage
	{
		public const uint protocolId = 6215;
		internal Boolean _isInitialized = false;
		public List<GameFightResumeSlaveInfo> slavesInfo;
		
		public GameFightResumeWithSlavesMessage()
		{
			this.slavesInfo = new List<GameFightResumeSlaveInfo>();
		}
		
		public GameFightResumeWithSlavesMessage(List<FightDispellableEffectExtendedInformations> arg1, List<GameActionMark> arg2, uint arg3, List<GameFightSpellCooldown> arg4, uint arg5, uint arg6, List<GameFightResumeSlaveInfo> arg7)
			: this()
		{
			initGameFightResumeWithSlavesMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 6215;
		}
		
		public GameFightResumeWithSlavesMessage initGameFightResumeWithSlavesMessage(List<FightDispellableEffectExtendedInformations> arg1, List<GameActionMark> arg2, uint arg3 = 0, List<GameFightSpellCooldown> arg4 = null, uint arg5 = 0, uint arg6 = 0, List<GameFightResumeSlaveInfo> arg7 = null)
		{
			base.initGameFightResumeMessage(arg1, arg2, arg3, arg4, arg5, arg6);
			this.slavesInfo = arg7;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.slavesInfo = new List<GameFightResumeSlaveInfo>();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightResumeWithSlavesMessage(arg1);
		}
		
		public void serializeAs_GameFightResumeWithSlavesMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightResumeMessage(arg1);
			arg1.WriteShort((short)this.slavesInfo.Count);
			var loc1 = 0;
			while ( loc1 < this.slavesInfo.Count )
			{
				this.slavesInfo[loc1].serializeAs_GameFightResumeSlaveInfo(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightResumeWithSlavesMessage(arg1);
		}
		
		public void deserializeAs_GameFightResumeWithSlavesMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new GameFightResumeSlaveInfo()) as GameFightResumeSlaveInfo).deserialize(arg1);
				this.slavesInfo.Add((GameFightResumeSlaveInfo)loc3);
				++loc2;
			}
		}
		
	}
}
