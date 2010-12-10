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
	
	public class TaxCollectorInformationsInWaitForHelpState : TaxCollectorInformations
	{
		public const uint protocolId = 166;
		public ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
		
		public TaxCollectorInformationsInWaitForHelpState()
		{
			this.waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
		}
		
		public TaxCollectorInformationsInWaitForHelpState(int arg1, uint arg2, uint arg3, AdditionalTaxCollectorInformations arg4, int arg5, int arg6, uint arg7, int arg8, EntityLook arg9, ProtectedEntityWaitingForHelpInfo arg10)
			: this()
		{
			initTaxCollectorInformationsInWaitForHelpState(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
		
		public override uint getTypeId()
		{
			return 166;
		}
		
		public TaxCollectorInformationsInWaitForHelpState initTaxCollectorInformationsInWaitForHelpState(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, AdditionalTaxCollectorInformations arg4 = null, int arg5 = 0, int arg6 = 0, uint arg7 = 0, int arg8 = 0, EntityLook arg9 = null, ProtectedEntityWaitingForHelpInfo arg10 = null)
		{
			base.initTaxCollectorInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			this.waitingForHelpInfo = arg10;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_TaxCollectorInformationsInWaitForHelpState(arg1);
		}
		
		public void serializeAs_TaxCollectorInformationsInWaitForHelpState(BigEndianWriter arg1)
		{
			base.serializeAs_TaxCollectorInformations(arg1);
			this.waitingForHelpInfo.serializeAs_ProtectedEntityWaitingForHelpInfo(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorInformationsInWaitForHelpState(arg1);
		}
		
		public void deserializeAs_TaxCollectorInformationsInWaitForHelpState(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
			this.waitingForHelpInfo.deserialize(arg1);
		}
		
	}
}
