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
	
	public class ObjectEffects : Object
	{
		public const uint protocolId = 358;
		public int powerRate = 0;
		public Boolean overMax = false;
		public List<ObjectEffect> effects;
		
		public ObjectEffects()
		{
			this.effects = new List<ObjectEffect>();
		}
		
		public ObjectEffects(int arg1, Boolean arg2, List<ObjectEffect> arg3)
			: this()
		{
			initObjectEffects(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 358;
		}
		
		public ObjectEffects initObjectEffects(int arg1 = 0, Boolean arg2 = false, List<ObjectEffect> arg3 = null)
		{
			this.powerRate = arg1;
			this.overMax = arg2;
			this.effects = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.powerRate = 0;
			this.overMax = false;
			this.effects = new List<ObjectEffect>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffects(arg1);
		}
		
		public void serializeAs_ObjectEffects(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.powerRate);
			arg1.WriteBoolean(this.overMax);
			arg1.WriteShort((short)this.effects.Count);
			var loc1 = 0;
			while ( loc1 < this.effects.Count )
			{
				arg1.WriteShort((short)this.effects[loc1].getTypeId());
				this.effects[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffects(arg1);
		}
		
		public void deserializeAs_ObjectEffects(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			this.powerRate = (int)arg1.ReadShort();
			this.overMax = (Boolean)arg1.ReadBoolean();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<ObjectEffect>((uint)loc3)) as ObjectEffect).deserialize(arg1);
				this.effects.Add((ObjectEffect)loc4);
				++loc2;
			}
		}
		
	}
}
