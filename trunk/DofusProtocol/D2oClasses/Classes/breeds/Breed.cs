

// Generated on 10/28/2013 14:03:17
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Breed", "com.ankamagames.dofus.datacenter.breeds")]
    [Serializable]
    public class Breed : IDataObject, IIndexedData
    {
        private const String MODULE = "Breeds";
        public int id;
        [I18NField]
        public uint shortNameId;
        [I18NField]
        public uint longNameId;
        [I18NField]
        public uint descriptionId;
        [I18NField]
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
        [D2OIgnore]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint ShortNameId
        {
            get { return shortNameId; }
            set { shortNameId = value; }
        }
        [D2OIgnore]
        public uint LongNameId
        {
            get { return longNameId; }
            set { longNameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        [D2OIgnore]
        public uint GameplayDescriptionId
        {
            get { return gameplayDescriptionId; }
            set { gameplayDescriptionId = value; }
        }
        [D2OIgnore]
        public String MaleLook
        {
            get { return maleLook; }
            set { maleLook = value; }
        }
        [D2OIgnore]
        public String FemaleLook
        {
            get { return femaleLook; }
            set { femaleLook = value; }
        }
        [D2OIgnore]
        public uint CreatureBonesId
        {
            get { return creatureBonesId; }
            set { creatureBonesId = value; }
        }
        [D2OIgnore]
        public int MaleArtwork
        {
            get { return maleArtwork; }
            set { maleArtwork = value; }
        }
        [D2OIgnore]
        public int FemaleArtwork
        {
            get { return femaleArtwork; }
            set { femaleArtwork = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForStrength
        {
            get { return statsPointsForStrength; }
            set { statsPointsForStrength = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForIntelligence
        {
            get { return statsPointsForIntelligence; }
            set { statsPointsForIntelligence = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForChance
        {
            get { return statsPointsForChance; }
            set { statsPointsForChance = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForAgility
        {
            get { return statsPointsForAgility; }
            set { statsPointsForAgility = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForVitality
        {
            get { return statsPointsForVitality; }
            set { statsPointsForVitality = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForWisdom
        {
            get { return statsPointsForWisdom; }
            set { statsPointsForWisdom = value; }
        }
        [D2OIgnore]
        public List<uint> BreedSpellsId
        {
            get { return breedSpellsId; }
            set { breedSpellsId = value; }
        }
        [D2OIgnore]
        public List<uint> MaleColors
        {
            get { return maleColors; }
            set { maleColors = value; }
        }
        [D2OIgnore]
        public List<uint> FemaleColors
        {
            get { return femaleColors; }
            set { femaleColors = value; }
        }
    }
}