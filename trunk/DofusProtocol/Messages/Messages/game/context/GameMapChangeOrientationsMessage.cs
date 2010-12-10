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
	
	public class GameMapChangeOrientationsMessage : Message
	{
		public const uint protocolId = 6155;
		internal Boolean _isInitialized = false;
		public List<ActorOrientation> orientations;
		
		public GameMapChangeOrientationsMessage()
		{
			this.orientations = new List<ActorOrientation>();
		}
		
		public GameMapChangeOrientationsMessage(List<ActorOrientation> arg1)
			: this()
		{
			initGameMapChangeOrientationsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6155;
		}
		
		public GameMapChangeOrientationsMessage initGameMapChangeOrientationsMessage(List<ActorOrientation> arg1)
		{
			this.orientations = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.orientations = new List<ActorOrientation>();
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
			this.serializeAs_GameMapChangeOrientationsMessage(arg1);
		}
		
		public void serializeAs_GameMapChangeOrientationsMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.orientations.Count);
			var loc1 = 0;
			while ( loc1 < this.orientations.Count )
			{
				this.orientations[loc1].serializeAs_ActorOrientation(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameMapChangeOrientationsMessage(arg1);
		}
		
		public void deserializeAs_GameMapChangeOrientationsMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ActorOrientation()) as ActorOrientation).deserialize(arg1);
				this.orientations.Add((ActorOrientation)loc3);
				++loc2;
			}
		}
		
	}
}
