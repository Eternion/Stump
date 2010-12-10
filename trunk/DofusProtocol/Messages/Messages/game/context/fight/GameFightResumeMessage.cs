// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightResumeMessage : GameFightSpectateMessage
	{
		public const uint protocolId = 6067;
		internal Boolean _isInitialized = false;
		public List<GameFightSpellCooldown> spellCooldowns;
		public uint summonCount = 0;
		
		public GameFightResumeMessage()
		{
			this.spellCooldowns = new List<GameFightSpellCooldown>();
		}
		
		public GameFightResumeMessage(List<FightDispellableEffectExtendedInformations> arg1, List<GameActionMark> arg2, uint arg3, List<GameFightSpellCooldown> arg4, uint arg5)
			: this()
		{
			initGameFightResumeMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6067;
		}
		
		public GameFightResumeMessage initGameFightResumeMessage(List<FightDispellableEffectExtendedInformations> arg1, List<GameActionMark> arg2, uint arg3 = 0, List<GameFightSpellCooldown> arg4 = null, uint arg5 = 0)
		{
			base.initGameFightSpectateMessage(arg1, arg2, arg3);
			this.spellCooldowns = arg4;
			this.summonCount = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.spellCooldowns = new List<GameFightSpellCooldown>();
			this.summonCount = 0;
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
			this.serializeAs_GameFightResumeMessage(arg1);
		}
		
		public void serializeAs_GameFightResumeMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightSpectateMessage(arg1);
			arg1.WriteShort((short)this.spellCooldowns.Count);
			var loc1 = 0;
			while ( loc1 < this.spellCooldowns.Count )
			{
				this.spellCooldowns[loc1].serializeAs_GameFightSpellCooldown(arg1);
				++loc1;
			}
			if ( this.summonCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.summonCount + ") on element summonCount.");
			}
			arg1.WriteByte((byte)this.summonCount);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightResumeMessage(arg1);
		}
		
		public void deserializeAs_GameFightResumeMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new GameFightSpellCooldown()) as GameFightSpellCooldown).deserialize(arg1);
				this.spellCooldowns.Add((GameFightSpellCooldown)loc3);
				++loc2;
			}
			this.summonCount = (uint)arg1.ReadByte();
			if ( this.summonCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.summonCount + ") on element of GameFightResumeMessage.summonCount.");
			}
		}
		
	}
}
