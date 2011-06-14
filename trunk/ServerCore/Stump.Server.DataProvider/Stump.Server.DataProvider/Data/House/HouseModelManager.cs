
using System.Collections.Generic;
using System.Linq;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.House
{
    //public class HouseModelManager : DataManager<int, HouseModel>
    //{

    //    protected override HouseModel InternalGetOne(int id)
    //    {
    //        var house = D2OLoader.LoadData<DofusProtocol.D2oClasses.House>(id);
    //        return new HouseModel {ModelId = house.typeId, DefaultPrice = house.defaultPrice};
    //    }

    //    protected override Dictionary<int, HouseModel> InternalGetAll()
    //    {
    //        return D2OLoader.LoadData<DofusProtocol.D2oClasses.House>()
    //         .Select(h => new HouseModel { ModelId = h.typeId, DefaultPrice = h.defaultPrice }).ToDictionary(h => h.ModelId);
    //    }
    //}
}