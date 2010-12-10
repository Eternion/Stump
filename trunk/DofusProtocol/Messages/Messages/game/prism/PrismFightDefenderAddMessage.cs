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
	
	public class PrismFightDefenderAddMessage : Message
	{
		public const uint protocolId = 5895;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations;
		public Boolean inMain = false;
		
		public PrismFightDefenderAddMessage()
		{
			this.fighterMovementInformations = new CharacterMinimalPlusLookAndGradeInformations();
		}
		
		public PrismFightDefenderAddMessage(double arg1, CharacterMinimalPlusLookAndGradeInformations arg2, Boolean arg3)
			: this()
		{
			initPrismFightDefenderAddMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5895;
		}
		
		public PrismFightDefenderAddMessage initPrismFightDefenderAddMessage(double arg1 = 0, CharacterMinimalPlusLookAndGradeInformations arg2 = null, Boolean arg3 = false)
		{
			this.fightId = arg1;
			this.fighterMovementInformations = arg2;
			this.inMain = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.fighterMovementInformations = new CharacterMinimalPlusLookAndGradeInformations();
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
			this.serializeAs_PrismFightDefenderAddMessage(arg1);
		}
		
		public void serializeAs_PrismFightDefenderAddMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			this.fighterMovementInformations.serializeAs_CharacterMinimalPlusLookAndGradeInformations(arg1);
			arg1.WriteBoolean(this.inMain);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightDefenderAddMessage(arg1);
		}
		
		public void deserializeAs_PrismFightDefenderAddMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			this.fighterMovementInformations = new CharacterMinimalPlusLookAndGradeInformations();
			this.fighterMovementInformations.deserialize(arg1);
			this.inMain = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
