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
	
	public class ShowCellSpectatorMessage : ShowCellMessage
	{
		public const uint protocolId = 6158;
		internal Boolean _isInitialized = false;
		public String playerName = "";
		
		public ShowCellSpectatorMessage()
		{
		}
		
		public ShowCellSpectatorMessage(int arg1, uint arg2, String arg3)
			: this()
		{
			initShowCellSpectatorMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6158;
		}
		
		public ShowCellSpectatorMessage initShowCellSpectatorMessage(int arg1 = 0, uint arg2 = 0, String arg3 = "")
		{
			base.initShowCellMessage(arg1, arg2);
			this.playerName = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.playerName = "";
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
			this.serializeAs_ShowCellSpectatorMessage(arg1);
		}
		
		public void serializeAs_ShowCellSpectatorMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ShowCellMessage(arg1);
			arg1.WriteUTF((string)this.playerName);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShowCellSpectatorMessage(arg1);
		}
		
		public void deserializeAs_ShowCellSpectatorMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.playerName = (String)arg1.ReadUTF();
		}
		
	}
}
