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
	
	public class ChallengeInfoMessage : Message
	{
		public const uint protocolId = 6022;
		internal Boolean _isInitialized = false;
		public uint challengeId = 0;
		public int targetId = 0;
		public uint @baseXpBonus = 0;
		public uint extraXpBonus = 0;
		public uint @baseDropBonus = 0;
		public uint extraDropBonus = 0;
		
		public ChallengeInfoMessage()
		{
		}
		
		public ChallengeInfoMessage(uint arg1, int arg2, uint arg3, uint arg4, uint arg5, uint arg6)
			: this()
		{
			initChallengeInfoMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 6022;
		}
		
		public ChallengeInfoMessage initChallengeInfoMessage(uint arg1 = 0, int arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0, uint arg6 = 0)
		{
			this.challengeId = arg1;
			this.targetId = arg2;
			this.@baseXpBonus = arg3;
			this.extraXpBonus = arg4;
			this.@baseDropBonus = arg5;
			this.extraDropBonus = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.challengeId = 0;
			this.targetId = 0;
			this.@baseXpBonus = 0;
			this.extraXpBonus = 0;
			this.@baseDropBonus = 0;
			this.extraDropBonus = 0;
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
			this.serializeAs_ChallengeInfoMessage(arg1);
		}
		
		public void serializeAs_ChallengeInfoMessage(BigEndianWriter arg1)
		{
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element challengeId.");
			}
			arg1.WriteByte((byte)this.challengeId);
			arg1.WriteInt((int)this.targetId);
			if ( this.@baseXpBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.@baseXpBonus + ") on element baseXpBonus.");
			}
			arg1.WriteInt((int)this.@baseXpBonus);
			if ( this.extraXpBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraXpBonus + ") on element extraXpBonus.");
			}
			arg1.WriteInt((int)this.extraXpBonus);
			if ( this.@baseDropBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.@baseDropBonus + ") on element baseDropBonus.");
			}
			arg1.WriteInt((int)this.@baseDropBonus);
			if ( this.extraDropBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraDropBonus + ") on element extraDropBonus.");
			}
			arg1.WriteInt((int)this.extraDropBonus);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChallengeInfoMessage(arg1);
		}
		
		public void deserializeAs_ChallengeInfoMessage(BigEndianReader arg1)
		{
			this.challengeId = (uint)arg1.ReadByte();
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element of ChallengeInfoMessage.challengeId.");
			}
			this.targetId = (int)arg1.ReadInt();
			this.@baseXpBonus = (uint)arg1.ReadInt();
			if ( this.@baseXpBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.@baseXpBonus + ") on element of ChallengeInfoMessage.baseXpBonus.");
			}
			this.extraXpBonus = (uint)arg1.ReadInt();
			if ( this.extraXpBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraXpBonus + ") on element of ChallengeInfoMessage.extraXpBonus.");
			}
			this.@baseDropBonus = (uint)arg1.ReadInt();
			if ( this.@baseDropBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.@baseDropBonus + ") on element of ChallengeInfoMessage.baseDropBonus.");
			}
			this.extraDropBonus = (uint)arg1.ReadInt();
			if ( this.extraDropBonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraDropBonus + ") on element of ChallengeInfoMessage.extraDropBonus.");
			}
		}
		
	}
}
