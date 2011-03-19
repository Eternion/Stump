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
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.Data.Breeds
{
    [Serializable]
    [ActiveRecord("breeds")]
    [AttributeAssociatedFile("Breeds")]
    public sealed class BreedRecord : DataBaseRecord<BreedRecord>
    {

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("AlternativeMaleSkin", ColumnType = "Serializable")]
        public List<uint> AlternativeMaleSkin
        {
            get;
            set;
        }

        [Property("AlternativeFemaleSkin", ColumnType = "Serializable")]
        public List<uint> AlternativeFemaleSkin
        {
            get;
            set;
        }

        [Property("GameplayDescriptionId")]
        public uint GameplayDescriptionId
        {
            get; 
            set;
        }

        [Property("ShortNameId")]
        public uint ShortNameId
        {
            get;
            set;
        }

        [Property("LongNameId")]
        public uint LongNameId
        {
            get;
            set;
        }

        [Property("DescriptionId")]
        public uint DescriptionId
        {
            get;
            set;
        }

        [Property("MaleLook")]
        public string MaleLook
        {
            get;
            set;
        }

        [Property("FemaleLook")]
        public string FemaleLook
        {
            get;
            set;
        }

        [Property("CreatureBonesId")]
        public uint CreatureBonesId
        {
            get;
            set;
        }

        [Property("MaleArtwork")]
        public int MaleArtwork
        {
            get;
            set;
        }

        [Property("FemaleArtwork")]
        public int FemaleArtwork
        {
            get;
            set;
        }

        [Property("StatsPointsForStrength", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForStrength
        {
            get;
            set;
        }

        [Property("StatsPointsForIntelligence", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForIntelligence
        {
            get;
            set;
        }

        [Property("StatsPointsForChance", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForChance
        {
            get;
            set;
        }

        [Property("StatsPointsForAgility", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForAgility
        {
            get;
            set;
        }

        [Property("StatsPointsForVitality", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForVitality
        {
            get;
            set;
        }

        [Property("StatsPointsForWisdom", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForWisdom
        {
            get;
            set;
        }

        [Property("MaleColors", ColumnType = "Serializable")]
        public List<uint> MaleColors
        {
            get;
            set;
        }

        [Property("FemaleColors", ColumnType = "Serializable")]
        public List<uint> FemaleColors
        {
            get;
            set;
        }

        [Property("BreedSpellsId", ColumnType = "Serializable")]
        public List<uint> BreedSpellsId
        {
            get;
            set;
        }
    }
}