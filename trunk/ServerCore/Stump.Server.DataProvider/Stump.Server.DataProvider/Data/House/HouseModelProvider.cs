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
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.House
{
    public class HouseModelProvider : DataProvider<int, HouseModel>
    {

        protected override HouseModel GetData(int id)
        {
            var house = D2OLoader.LoadData<DofusProtocol.D2oClasses.House>(id);
            return new HouseModel {ModelId = house.typeId, DefaultPrice = house.defaultPrice};
        }

        protected override Dictionary<int, HouseModel> GetAllData()
        {
            return D2OLoader.LoadData<DofusProtocol.D2oClasses.House>()
             .Select(h => new HouseModel { ModelId = h.typeId, DefaultPrice = h.defaultPrice }).ToDictionary(h => h.ModelId);
        }
    }
}