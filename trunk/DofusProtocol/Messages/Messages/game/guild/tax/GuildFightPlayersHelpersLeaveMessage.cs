using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildFightPlayersHelpersLeaveMessage : Message
	{
		public const uint protocolId = 5719;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public uint playerId = 0;
		
		public GuildFightPlayersHelpersLeaveMessage()
		{
		}
		
		public GuildFightPlayersHelpersLeaveMessage(double arg1, uint arg2)
			: this()
		{
			initGuildFightPlayersHelpersLeaveMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5719;
		}
		
		public GuildFightPlayersHelpersLeaveMessage initGuildFightPlayersHelpersLeaveMessage(double arg1 = 0, uint arg2 = 0)
		{
			this.fightId = arg1;
			this.playerId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.playerId = 0;
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
			this.serializeAs_GuildFightPlayersHelpersLeaveMessage(arg1);
		}
		
		public void serializeAs_GuildFightPlayersHelpersLeaveMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteDouble(this.fightId);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildFightPlayersHelpersLeaveMessage(arg1);
		}
		
		public void deserializeAs_GuildFightPlayersHelpersLeaveMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of GuildFightPlayersHelpersLeaveMessage.fightId.");
			}
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of GuildFightPlayersHelpersLeaveMessage.playerId.");
			}
		}
		
	}
}
