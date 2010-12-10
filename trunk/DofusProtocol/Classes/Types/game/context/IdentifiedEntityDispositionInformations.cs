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
	
	public class IdentifiedEntityDispositionInformations : EntityDispositionInformations
	{
		public const uint protocolId = 107;
		public int id = 0;
		
		public IdentifiedEntityDispositionInformations()
		{
		}
		
		public IdentifiedEntityDispositionInformations(int arg1, uint arg2, int arg3)
			: this()
		{
			initIdentifiedEntityDispositionInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 107;
		}
		
		public IdentifiedEntityDispositionInformations initIdentifiedEntityDispositionInformations(int arg1 = 0, uint arg2 = 1, int arg3 = 0)
		{
			base.initEntityDispositionInformations(arg1, arg2);
			this.id = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.id = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_IdentifiedEntityDispositionInformations(arg1);
		}
		
		public void serializeAs_IdentifiedEntityDispositionInformations(BigEndianWriter arg1)
		{
			base.serializeAs_EntityDispositionInformations(arg1);
			arg1.WriteInt((int)this.id);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentifiedEntityDispositionInformations(arg1);
		}
		
		public void deserializeAs_IdentifiedEntityDispositionInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.id = (int)arg1.ReadInt();
		}
		
	}
}
