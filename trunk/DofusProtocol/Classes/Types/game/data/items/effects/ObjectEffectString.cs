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
	
	public class ObjectEffectString : ObjectEffect
	{
		public const uint protocolId = 74;
		public String value = "";
		
		public ObjectEffectString()
		{
		}
		
		public ObjectEffectString(uint arg1, String arg2)
			: this()
		{
			initObjectEffectString(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 74;
		}
		
		public ObjectEffectString initObjectEffectString(uint arg1 = 0, String arg2 = "")
		{
			base.initObjectEffect(arg1);
			this.value = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.value = "";
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectString(arg1);
		}
		
		public void serializeAs_ObjectEffectString(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			arg1.WriteUTF((string)this.value);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectString(arg1);
		}
		
		public void deserializeAs_ObjectEffectString(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.value = (String)arg1.ReadUTF();
		}
		
	}
}
