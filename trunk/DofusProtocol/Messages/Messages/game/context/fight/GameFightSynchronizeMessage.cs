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
