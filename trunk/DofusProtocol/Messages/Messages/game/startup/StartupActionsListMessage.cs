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
	
	public class StartupActionsListMessage : Message
	{
		public const uint protocolId = 1301;
		internal Boolean _isInitialized = false;
		public List<StartupActionAddObject> actions;
		
		public StartupActionsListMessage()
		{
			this.actions = new List<StartupActionAddObject>();
		}
		
		public StartupActionsListMessage(List<StartupActionAddObject> arg1)
			: this()
		{
			initStartupActionsListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1301;
		}
		
		public StartupActionsListMessage initStartupActionsListMessage(List<StartupActionAddObject> arg1)
		{
			this.actions = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.actions = new List<StartupActionAddObject>();
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
			this.serializeAs_StartupActionsListMessage(arg1);
		}
		
		public void serializeAs_StartupActionsListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.actions.Count);
			var loc1 = 0;
			while ( loc1 < this.actions.Count )
			{
				this.actions[loc1].serializeAs_StartupActionAddObject(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StartupActionsListMessage(arg1);
		}
		
		public void deserializeAs_StartupActionsListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new StartupActionAddObject()) as StartupActionAddObject).deserialize(arg1);
				this.actions.Add((StartupActionAddObject)loc3);
				++loc2;
			}
		}
		
	}
}
