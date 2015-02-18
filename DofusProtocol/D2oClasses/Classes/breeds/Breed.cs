

// Generated on 02/18/2015 11:05:56
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
        public const String MODULE = "Breeds";
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
        public List<BreedRoleByBreed> breedRoles;
        public List<uint> maleColors;
        public List<uint> femaleColors;
        public uint spawnMap;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public uint ShortNameId
        {
            get { return this.shortNameId; }
            set { this.shortNameId = value; }
        }
        [D2OIgnore]
        public uint LongNameId
        {
            get { return this.longNameId; }
            set { this.longNameId = value; }
        }
        [D2OIgnore]
        public uint DescriptionId
        {
            get { return this.descriptionId; }
            set { this.descriptionId = value; }
        }
        [D2OIgnore]
        public uint GameplayDescriptionId
        {
            get { return this.gameplayDescriptionId; }
            set { this.gameplayDescriptionId = value; }
        }
        [D2OIgnore]
        public String MaleLook
        {
            get { return this.maleLook; }
            set { this.maleLook = value; }
        }
        [D2OIgnore]
        public String FemaleLook
        {
            get { return this.femaleLook; }
            set { this.femaleLook = value; }
        }
        [D2OIgnore]
        public uint CreatureBonesId
        {
            get { return this.creatureBonesId; }
            set { this.creatureBonesId = value; }
        }
        [D2OIgnore]
        public int MaleArtwork
        {
            get { return this.maleArtwork; }
            set { this.maleArtwork = value; }
        }
        [D2OIgnore]
        public int FemaleArtwork
        {
            get { return this.femaleArtwork; }
            set { this.femaleArtwork = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForStrength
        {
            get { return this.statsPointsForStrength; }
            set { this.statsPointsForStrength = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForIntelligence
        {
            get { return this.statsPointsForIntelligence; }
            set { this.statsPointsForIntelligence = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForChance
        {
            get { return this.statsPointsForChance; }
            set { this.statsPointsForChance = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForAgility
        {
            get { return this.statsPointsForAgility; }
            set { this.statsPointsForAgility = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForVitality
        {
            get { return this.statsPointsForVitality; }
            set { this.statsPointsForVitality = value; }
        }
        [D2OIgnore]
        public List<List<uint>> StatsPointsForWisdom
        {
            get { return this.statsPointsForWisdom; }
            set { this.statsPointsForWisdom = value; }
        }
        [D2OIgnore]
        public List<uint> BreedSpellsId
        {
            get { return this.breedSpellsId; }
            set { this.breedSpellsId = value; }
        }
        [D2OIgnore]
        public List<BreedRoleByBreed> BreedRoles
        {
            get { return this.breedRoles; }
            set { this.breedRoles = value; }
        }
        [D2OIgnore]
        public List<uint> MaleColors
        {
            get { return this.maleColors; }
            set { this.maleColors = value; }
        }
        [D2OIgnore]
        public List<uint> FemaleColors
        {
            get { return this.femaleColors; }
            set { this.femaleColors = value; }
        }
        [D2OIgnore]
        public uint SpawnMap
        {
            get { return this.spawnMap; }
            set { this.spawnMap = value; }
        }
    }
}