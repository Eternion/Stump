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
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Core;
using Stump.Server.DataProvider.Data.D2oTool;

namespace Stump.Server.DataProvider.Data.Breeds
{
    public class BreedTemplateManager : DataManager<PlayableBreedEnum, BreedTemplate>
    {
        /// <summary>
        ///   Name of Breed file
        /// </summary>
        [Variable]
        public static string BreedFile = "Breeds.xml";

        protected override BreedTemplate GetData(PlayableBreedEnum id)
        {
            Breed breedData = D2OLoader.LoadData<Breed>().FirstOrDefault(b => b.id == (int) id);
            Serializer.DeserializeItems<>()
            if (breedData == null)
                throw new Exception("The correspondant D2O file \'Breeds.d2o\' is unfundable");

            using (var reader = new StreamReader(Settings.StaticPath + BreedFile))
            {
                BreedTemplate template =
                    Serializer.Deserialize<List<BreedTemplate>>(reader.BaseStream).FirstOrDefault(t => t.Id == id);

                if (template == null)
                    throw new Exception(string.Format("The correspondant xml file {0} is unfundable", BreedFile));

                template.MaleLook = breedData.maleLook.ToEntityLook();
                template.MaleColors = breedData.maleColors;
                template.FemaleLook = breedData.femaleLook.ToEntityLook();
                template.FemaleColors = breedData.femaleColors;
                template.StatsPointsForAgility = breedData.statsPointsForAgility;
                template.StatsPointsForChance = breedData.statsPointsForChance;
                template.StatsPointsForIntelligence = breedData.statsPointsForIntelligence;
                template.StatsPointsForStrength = breedData.statsPointsForStrength;
                template.StatsPointsForVitality = breedData.statsPointsForVitality;
                template.StatsPointsForWisdom = breedData.statsPointsForWisdom;

                return template;
            }
        }

        protected override Dictionary<PlayableBreedEnum, BreedTemplate> GetAllData()
        {
            Dictionary<int, Breed> breedDatas = D2OLoader.LoadData<Breed>().ToDictionary(b => b.id);
            using (var reader = new StreamReader(Settings.StaticPath + BreedFile))
            {
                var templates = Serializer.Deserialize<List<BreedTemplate>>(reader.BaseStream);

                foreach (BreedTemplate template in templates)
                {
                    if (!breedDatas.ContainsKey((int) template.Id))
                        throw new Exception("The correspondant D2O file \'Breeds.d2o\' is unfundable");

                    Breed breedData = breedDatas[(int) template.Id];
                    template.MaleLook = breedData.maleLook.ToEntityLook();
                    template.MaleColors = breedData.maleColors;
                    template.FemaleLook = breedData.femaleLook.ToEntityLook();
                    template.FemaleColors = breedData.femaleColors;
                    template.StatsPointsForAgility = breedData.statsPointsForAgility;
                    template.StatsPointsForChance = breedData.statsPointsForChance;
                    template.StatsPointsForIntelligence = breedData.statsPointsForIntelligence;
                    template.StatsPointsForStrength = breedData.statsPointsForStrength;
                    template.StatsPointsForVitality = breedData.statsPointsForVitality;
                    template.StatsPointsForWisdom = breedData.statsPointsForWisdom;
                }
                return templates.ToDictionary(b => b.Id);
            }
        }
    }
}