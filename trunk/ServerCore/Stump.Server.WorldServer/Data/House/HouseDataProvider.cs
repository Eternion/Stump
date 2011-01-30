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
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Xml;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.House
{
    public static class HouseDataProvider
    {
        /// <summary>
        ///   Name of House Doors file
        /// </summary>
        [Variable]
        public static string HousesDoorsFile = "Houses/HousesDoors.xml";

        private static Dictionary<int, HouseModel> m_houseModels;

        private static Dictionary<int, List<int>> m_housesDoors;


        [StageStep(Stages.One, "Loaded Houses Model")]
        public static void LoadHousesModel()
        {
            m_houseModels = DataLoader.LoadData<DofusProtocol.D2oClasses.House>()
                .Select(h => new HouseModel { ModelId = h.typeId, DefaultPrice = h.defaultPrice }).ToDictionary(h => h.ModelId);
        }

        [StageStep(Stages.One, "Loaded Houses Doors")]
        public static void LoadHousesDoors()
        {
            var list = XmlUtils.Deserialize<List<HouseDoors>>(Settings.StaticPath + HousesDoorsFile);

            m_housesDoors = new Dictionary<int, List<int>>(list.Count);
            foreach (var element in list)
                m_housesDoors.Add(element.HouseId, element.Doors);
        }

        public static HouseModel GetHouseModel(int modelId)
        {
            if (m_houseModels.ContainsKey(modelId))
                return m_houseModels[modelId];
            return null;
        }

        public static List<int> GetHouseDoors(int houseId)
        {
            if (m_housesDoors.ContainsKey(houseId))
                return m_housesDoors[houseId];
            return null;
        }
    }
}