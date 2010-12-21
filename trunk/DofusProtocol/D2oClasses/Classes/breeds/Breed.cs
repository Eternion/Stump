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
namespace Stump.DofusProtocol.D2oClasses
{
	
	[AttributeAssociatedFile("Breeds")]
	public class Breed : Object
	{
		internal const String MODULE = "Breeds";
		public List<uint> alternativeMaleSkin;
		public List<uint> alternativeFemaleSkin;
		public uint gameplayDescriptionId;
		public int id;
		public uint shortNameId;
		public uint longNameId;
		public uint descriptionId;
		public String maleLook;
		public String femaleLook;
		public uint creatureBonesId;
		public int maleArtwork;
		public int femaleArtwork;
		public List<List<uint>> statsPointsForStrength;
		public List<List<uint>> statsPointsForIntelligence;
		public List<List<uint>> statsPointsForChance;
		public List<List<uint>> statsPointsForAgility;
		public List<uint> maleColors;
		internal Array _skinsForBreed;
		public List<List<uint>> statsPointsForVitality;
		public List<List<uint>> statsPointsForWisdom;
		public List<uint> breedSpellsId;
		public List<uint> femaleColors;
		
	}
}
