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
	
	public class EntityDispositionInformations : Object
	{
		public const uint protocolId = 60;
		public int cellId = 0;
		public uint direction = 1;
		
		public EntityDispositionInformations()
		{
		}
		
		public EntityDispositionInformations(int arg1, uint arg2)
			: this()
		{
			initEntityDispositionInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 60;
		}
		
		public EntityDispositionInformations initEntityDispositionInformations(int arg1 = 0, uint arg2 = 1)
		{
			this.cellId = arg1;
			this.direction = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.cellId = 0;
			this.direction = 1;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_EntityDispositionInformations(arg1);
		}
		
		public void serializeAs_EntityDispositionInformations(BigEndianWriter arg1)
		{
			if ( this.cellId < -1 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
			arg1.WriteByte((byte)this.direction);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EntityDispositionInformations(arg1);
		}
		
		public void deserializeAs_EntityDispositionInformations(BigEndianReader arg1)
		{
			this.cellId = (int)arg1.ReadShort();
			if ( this.cellId < -1 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of EntityDispositionInformations.cellId.");
			}
			this.direction = (uint)arg1.ReadByte();
			if ( this.direction < 0 )
			{
				throw new Exception("Forbidden value (" + this.direction + ") on element of EntityDispositionInformations.direction.");
			}
		}
		
	}
}
