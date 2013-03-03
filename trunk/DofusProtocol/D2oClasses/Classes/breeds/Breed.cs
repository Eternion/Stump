
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Breeds")]
    [Serializable]
    public class Breed : IDataObject, IIndexedData
    {
        private const String MODULE = "Breeds";
        public int id;
        public uint shortNameId;
        public uint longNameId;
        public uint descriptionId;
        public uint gameplayDescriptionId;
        public String maleLook;
        public String femaleLook;
        public uint creatureBonesId;
        public int maleArtwork;
        public int femaleArtwork;
        public List<List<uint>> statsPointsForStrength;
        public List<List<uint>> statsPointsForIntelligence;
        public List<List<uint>> statsPointsForChance;
        public List<List<uint>> statsPointsForAgility;
        public List<List<uint>> statsPointsForVitality;
        public List<List<uint>> statsPointsForWisdom;
        public List<uint> breedSpellsId;
        public List<uint> maleColors;
        public List<uint> femaleColors;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint ShortNameId
        {
            get { return shortNameId; }
            set { shortNameId = value; }
        }

        public uint LongNameId
        {
            get { return longNameId; }
            set { longNameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public uint GameplayDescriptionId
        {
            get { return gameplayDescriptionId; }
            set { gameplayDescriptionId = value; }
        }

        public String MaleLook
        {
            get { return maleLook; }
            set { maleLook = value; }
        }

        public String FemaleLook
        {
            get { return femaleLook; }
            set { femaleLook = value; }
        }

        public uint CreatureBonesId
        {
            get { return creatureBonesId; }
            set { creatureBonesId = value; }
        }

        public int MaleArtwork
        {
            get { return maleArtwork; }
            set { maleArtwork = value; }
        }

        public int FemaleArtwork
        {
            get { return femaleArtwork; }
            set { femaleArtwork = value; }
        }

        public List<List<uint>> StatsPointsForStrength
        {
            get { return statsPointsForStrength; }
            set { statsPointsForStrength = value; }
        }

        public List<List<uint>> StatsPointsForIntelligence
        {
            get { return statsPointsForIntelligence; }
            set { statsPointsForIntelligence = value; }
        }

        public List<List<uint>> StatsPointsForChance
        {
            get { return statsPointsForChance; }
            set { statsPointsForChance = value; }
        }

        public List<List<uint>> StatsPointsForAgility
        {
            get { return statsPointsForAgility; }
            set { statsPointsForAgility = value; }
        }

        public List<List<uint>> StatsPointsForVitality
        {
            get { return statsPointsForVitality; }
            set { statsPointsForVitality = value; }
        }

        public List<List<uint>> StatsPointsForWisdom
        {
            get { return statsPointsForWisdom; }
            set { statsPointsForWisdom = value; }
        }

        public List<uint> BreedSpellsId
        {
            get { return breedSpellsId; }
            set { breedSpellsId = value; }
        }

        public List<uint> MaleColors
        {
            get { return maleColors; }
            set { maleColors = value; }
        }

        public List<uint> FemaleColors
        {
            get { return femaleColors; }
            set { femaleColors = value; }
        }

    }
}