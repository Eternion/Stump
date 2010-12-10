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
	
	public class TaxCollectorFightersInformation : Object
	{
		public const uint protocolId = 169;
		public int collectorId = 0;
		public List<CharacterMinimalPlusLookInformations> allyCharactersInformations;
		public List<CharacterMinimalPlusLookInformations> enemyCharactersInformations;
		
		public TaxCollectorFightersInformation()
		{
			this.allyCharactersInformations = new List<CharacterMinimalPlusLookInformations>();
			this.enemyCharactersInformations = new List<CharacterMinimalPlusLookInformations>();
		}
		
		public TaxCollectorFightersInformation(int arg1, List<CharacterMinimalPlusLookInformations> arg2, List<CharacterMinimalPlusLookInformations> arg3)
			: this()
		{
			initTaxCollectorFightersInformation(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 169;
		}
		
		public TaxCollectorFightersInformation initTaxCollectorFightersInformation(int arg1 = 0, List<CharacterMinimalPlusLookInformations> arg2 = null, List<CharacterMinimalPlusLookInformations> arg3 = null)
		{
			this.collectorId = arg1;
			this.allyCharactersInformations = arg2;
			this.enemyCharactersInformations = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.collectorId = 0;
			this.allyCharactersInformations = new List<CharacterMinimalPlusLookInformations>();
			this.enemyCharactersInformations = new List<CharacterMinimalPlusLookInformations>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_TaxCollectorFightersInformation(arg1);
		}
		
		public void serializeAs_TaxCollectorFightersInformation(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.collectorId);
			arg1.WriteShort((short)this.allyCharactersInformations.Count);
			var loc1 = 0;
			while ( loc1 < this.allyCharactersInformations.Count )
			{
				this.allyCharactersInformations[loc1].serializeAs_CharacterMinimalPlusLookInformations(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.enemyCharactersInformations.Count);
			var loc2 = 0;
			while ( loc2 < this.enemyCharactersInformations.Count )
			{
				this.enemyCharactersInformations[loc2].serializeAs_CharacterMinimalPlusLookInformations(arg1);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorFightersInformation(arg1);
		}
		
		public void deserializeAs_TaxCollectorFightersInformation(BigEndianReader arg1)
		{
			object loc5 = null;
			object loc6 = null;
			this.collectorId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc5 = new CharacterMinimalPlusLookInformations()) as CharacterMinimalPlusLookInformations).deserialize(arg1);
				this.allyCharactersInformations.Add((CharacterMinimalPlusLookInformations)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				((loc6 = new CharacterMinimalPlusLookInformations()) as CharacterMinimalPlusLookInformations).deserialize(arg1);
				this.enemyCharactersInformations.Add((CharacterMinimalPlusLookInformations)loc6);
				++loc4;
			}
		}
		
	}
}
