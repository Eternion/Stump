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
using System.Collections.Generic;

namespace Stump.DofusProtocol.D2oClasses
{
    [AttributeAssociatedFile("Breeds")]
    public class Breed
    {
        public List<int> breedSpellsId;
        public int creatureBonesId;
        public int descriptionId;
        public int femaleArtwork;
        public List<int> femaleColors;
        public string femaleLook;
        public int gameplayDescriptionId;
        public int id;
        public int longNameId;
        public int maleArtwork;
        public List<int> maleColors;
        public string maleLook;
        public int shortNameId;
        public List<List<int>> statsPointsForAgility;
        public List<List<int>> statsPointsForChance;
        public List<List<int>> statsPointsForIntelligence;
        public List<List<int>> statsPointsForStrength;
        public List<List<int>> statsPointsForVitality;
        public List<List<int>> statsPointsForWisdom;
    }
}