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
using System.Linq;
using System.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Spells;
using BreedData = Stump.DofusProtocol.D2oClasses.Breed;

namespace Stump.Server.WorldServer.Breeds
{
    public abstract class BaseBreed
    {
        private readonly Dictionary<CaracteristicsIdEnum, PropertyInfo> m_statsThresholds;

        #region Fields

        public abstract BreedEnum Id
        {
            get;
        }

        // TODO : Start Item, Spells, redefinitions
        //private List<Item> m_StartItems;

        #endregion

        protected BaseBreed()
        {
            MaleColors = new List<int>();
            FemaleColors = new List<int>();
            StartSpells = new Dictionary<SpellIdEnum, int>();

            Name = GetType().Name.Replace("Breed", string.Empty);

            m_statsThresholds = new Dictionary<CaracteristicsIdEnum, PropertyInfo>
                {
                    {CaracteristicsIdEnum.Strength, GetType().GetProperty("StatsPointsForStrength")},
                    {CaracteristicsIdEnum.Chance, GetType().GetProperty("StatsPointsForChance")},
                    {CaracteristicsIdEnum.Intelligence, GetType().GetProperty("StatsPointsForIntelligence")},
                    {CaracteristicsIdEnum.Agility, GetType().GetProperty("StatsPointsForAgility")},
                    {CaracteristicsIdEnum.Wisdom, GetType().GetProperty("StatsPointsForWisdom")},
                    {CaracteristicsIdEnum.Vitality, GetType().GetProperty("StatsPointsForVitality")},
                };
        }

        public void Initialize(BreedData breeddata)
        {
            // Init BreedEnum's Colors
            MaleColors = breeddata.maleColors;
            FemaleColors = breeddata.femaleColors;

            StatsPointsForStrength = breeddata.statsPointsForStrength;
            StatsPointsForIntelligence = breeddata.statsPointsForIntelligence;
            StatsPointsForChance = breeddata.statsPointsForChance;
            StatsPointsForAgility = breeddata.statsPointsForAgility;
            StatsPointsForVitality = breeddata.statsPointsForVitality;
            StatsPointsForWisdom = breeddata.statsPointsForWisdom;

            BreedSpells = SpellManager.GetSpells(breeddata.breedSpellsId.ToArray());

            // This is a common spell to every breed.
            StartSpells.Add(SpellIdEnum.Punch, 64);

            // Start spells there...
            OnInitialize();
        }

        protected abstract void OnInitialize();

        public int GetNeededPointForStats(int actualpoints, CaracteristicsIdEnum stats)
        {
            var thresholds = (List<List<int>>) m_statsThresholds[stats].GetValue(this, new object[0]);

            return GetPoints(actualpoints, thresholds);
        }

        protected int GetPoints(int actualpoints, List<List<int>> thresholds)
        {
            for (int i = 0; i < thresholds.Count - 1; i++)
            {
                if (thresholds[i][0] < actualpoints &&
                    thresholds[i + 1][0] > actualpoints)
                    return thresholds[i][1];
            }

            return thresholds.Last()[1];
        }

        #region Properties

        public virtual int Scale
        {
            get { return 150; }
        }

        public Dictionary<SpellIdEnum, int> StartSpells
        {
            get;
            protected set;
        }

        public List<Spell> BreedSpells
        {
            get;
            private set;
        }

        public List<int> MaleColors
        {
            get;
            protected set;
        }

        public List<int> FemaleColors
        {
            get;
            protected set;
        }

        public List<List<int>> StatsPointsForStrength
        {
            get;
            protected set;
        }

        public List<List<int>> StatsPointsForIntelligence
        {
            get;
            protected set;
        }

        public List<List<int>> StatsPointsForChance
        {
            get;
            protected set;
        }

        public List<List<int>> StatsPointsForAgility
        {
            get;
            protected set;
        }

        public List<List<int>> StatsPointsForWisdom
        {
            get;
            protected set;
        }

        public List<List<int>> StatsPointsForVitality
        {
            get;
            protected set;
        }

        public virtual int StartLevel
        {
            get { return 1; }
        }

        public virtual int StartKamas
        {
            get { return 0; }
        }

        /// <summary>
        ///   This is where you begin.
        /// </summary>
        public virtual MapIdEnum StartMap
        {
            get { return MapIdEnum.Map_IncarnamStart; }
        }

        public virtual int StartHealthPoint
        {
            get { return 42; }
        }

        /// <summary>
        ///   This is where you begin.
        /// </summary>
        public virtual int StartCellId
        {
            get { return 257; }
        }

        /// <summary>
        ///   We read BreedEnum Name from our source's filename directly.
        ///   We could also read from our BreedEnum Enum.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        #endregion
    }
}