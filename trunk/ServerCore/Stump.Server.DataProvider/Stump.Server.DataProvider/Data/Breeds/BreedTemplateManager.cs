
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Breeds
{
    //[DataManager("Loading breeds ...", LoadPriority.Six)]
    //public class BreedTemplateManager : DataManager<PlayableBreedEnum, BreedTemplate>
    //{
    //    /// <summary>
    //    ///   Name of Breed file
    //    /// </summary>
    //    [Variable]
    //    public static string BreedFile = "Breeds.xml";

    //    protected override BreedTemplate InternalGetOne(PlayableBreedEnum id)
    //    {
    //        Breed breedData = D2OLoader.LoadData<Breed>().FirstOrDefault(b => b.id == (int) id);

    //        if (breedData == null)
    //            throw new Exception("The correspondant D2O file \'Breeds.d2o\' is unfundable");

    //        using (var reader = new StreamReader(Settings.StaticPath + BreedFile))
    //        {
                
    //            BreedTemplate template =
    //                XmlUtils.Deserialize<List<BreedTemplate>>(reader.BaseStream).FirstOrDefault(t => t.Id == id);

    //            if (template == null)
    //                throw new Exception(string.Format("The correspondant xml file {0} is unfundable", BreedFile));

    //            template.MaleLook = breedData.maleLook.ToEntityLook();
    //            template.MaleColors = breedData.maleColors;
    //            template.FemaleLook = breedData.femaleLook.ToEntityLook();
    //            template.FemaleColors = breedData.femaleColors;
    //            template.StatsPointsForAgility = breedData.statsPointsForAgility;
    //            template.StatsPointsForChance = breedData.statsPointsForChance;
    //            template.StatsPointsForIntelligence = breedData.statsPointsForIntelligence;
    //            template.StatsPointsForStrength = breedData.statsPointsForStrength;
    //            template.StatsPointsForVitality = breedData.statsPointsForVitality;
    //            template.StatsPointsForWisdom = breedData.statsPointsForWisdom;

    //            return template;
    //        }
    //    }

    //    protected override Dictionary<PlayableBreedEnum, BreedTemplate> InternalGetAll()
    //    {
    //        Dictionary<int, Breed> breedDatas = D2OLoader.LoadData<Breed>().ToDictionary(b => b.id);
    //        using (var reader = new StreamReader(Settings.StaticPath + BreedFile))
    //        {
    //            var templates = XmlUtils.Deserialize<List<BreedTemplate>>(reader.BaseStream);

    //            foreach (BreedTemplate template in templates)
    //            {
    //                if (!breedDatas.ContainsKey((int) template.Id))
    //                    throw new Exception("The correspondant D2O file \'Breeds.d2o\' is unfundable");

    //                Breed breedData = breedDatas[(int) template.Id];
    //                template.MaleLook = breedData.maleLook.ToEntityLook();
    //                template.MaleColors = breedData.maleColors;
    //                template.FemaleLook = breedData.femaleLook.ToEntityLook();
    //                template.FemaleColors = breedData.femaleColors;
    //                template.StatsPointsForAgility = breedData.statsPointsForAgility;
    //                template.StatsPointsForChance = breedData.statsPointsForChance;
    //                template.StatsPointsForIntelligence = breedData.statsPointsForIntelligence;
    //                template.StatsPointsForStrength = breedData.statsPointsForStrength;
    //                template.StatsPointsForVitality = breedData.statsPointsForVitality;
    //                template.StatsPointsForWisdom = breedData.statsPointsForWisdom;
    //            }
    //            return templates.ToDictionary(b => b.Id);
    //        }
    //    }

    //}
}