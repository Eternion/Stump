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
	
	public class GameEntitiesDispositionMessage : Message
	{
		public const uint protocolId = 5696;
		internal Boolean _isInitialized = false;
		public List<IdentifiedEntityDispositionInformations> dispositions;
		
		public GameEntitiesDispositionMessage()
		{
			this.dispositions = new List<IdentifiedEntityDispositionInformations>();
		}
		
		public GameEntitiesDispositionMessage(List<IdentifiedEntityDispositionInformations> arg1)
			: this()
		{
			initGameEntitiesDispositionMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5696;
		}
		
		public GameEntitiesDispositionMessage initGameEntitiesDispositionMessage(List<IdentifiedEntityDispositionInformations> arg1)
		{
			this.dispositions = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.dispositions = new List<IdentifiedEntityDispositionInformations>();
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
			this.serializeAs_GameEntitiesDispositionMessage(arg1);
		}
		
		public void serializeAs_GameEntitiesDispositionMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.dispositions.Count);
			var loc1 = 0;
			while ( loc1 < this.dispositions.Count )
			{
				arg1.WriteShort((short)this.dispositions[loc1].getTypeId());
				this.dispositions[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameEntitiesDispositionMessage(arg1);
		}
		
		public void deserializeAs_GameEntitiesDispositionMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<IdentifiedEntityDispositionInformations>((uint)loc3)) as IdentifiedEntityDispositionInformations).deserialize(arg1);
				this.dispositions.Add((IdentifiedEntityDispositionInformations)loc4);
				++loc2;
			}
		}
		
	}
}
