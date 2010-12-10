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
namespace Stump.DofusProtocol.Classes
{
	
	public class IgnoredInformations : Object
	{
		public const uint protocolId = 106;
		public String name = "";
		public uint id = 0;
		
		public IgnoredInformations()
		{
		}
		
		public IgnoredInformations(String arg1, uint arg2)
			: this()
		{
			initIgnoredInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 106;
		}
		
		public IgnoredInformations initIgnoredInformations(String arg1 = "", uint arg2 = 0)
		{
			this.name = arg1;
			this.id = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.name = "";
			this.id = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_IgnoredInformations(arg1);
		}
		
		public void serializeAs_IgnoredInformations(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteInt((int)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IgnoredInformations(arg1);
		}
		
		public void deserializeAs_IgnoredInformations(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.id = (uint)arg1.ReadInt();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of IgnoredInformations.id.");
			}
		}
		
	}
}
