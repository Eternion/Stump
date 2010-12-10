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
	
	public class IdentificationMessage : Message
	{
		public const uint protocolId = 4;
		internal Boolean _isInitialized = false;
		public Stump.DofusProtocol.Classes.Version version;
		public String login = "";
		public String password = "";
		public Boolean autoconnect = false;
		
		public IdentificationMessage()
		{
			this.version = new Stump.DofusProtocol.Classes.Version();
		}
		
		public IdentificationMessage(Stump.DofusProtocol.Classes.Version arg1, String arg2, String arg3, Boolean arg4)
			: this()
		{
			initIdentificationMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 4;
		}
		
		public IdentificationMessage initIdentificationMessage(Stump.DofusProtocol.Classes.Version arg1 = null, String arg2 = "", String arg3 = "", Boolean arg4 = false)
		{
			this.version = arg1;
			this.login = arg2;
			this.password = arg3;
			this.autoconnect = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.version = new Stump.DofusProtocol.Classes.Version();
			this.password = "";
			this.autoconnect = false;
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
			this.serializeAs_IdentificationMessage(arg1);
		}
		
		public void serializeAs_IdentificationMessage(BigEndianWriter arg1)
		{
			this.version.serializeAs_Version(arg1);
			arg1.WriteUTF((string)this.login);
			arg1.WriteUTF((string)this.password);
			arg1.WriteBoolean(this.autoconnect);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationMessage(arg1);
		}
		
		public void deserializeAs_IdentificationMessage(BigEndianReader arg1)
		{
			this.version = new Stump.DofusProtocol.Classes.Version();
			this.version.deserialize(arg1);
			this.login = (String)arg1.ReadUTF();
			this.password = (String)arg1.ReadUTF();
			this.autoconnect = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
