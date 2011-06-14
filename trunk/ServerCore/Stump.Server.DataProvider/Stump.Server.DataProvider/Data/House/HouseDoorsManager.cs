
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.House
{
    //public class HouseDoorsManager : DataManager<int, List<short>>
    //{
    //    /// <summary>
    //    ///   Name of House Doors file
    //    /// </summary>
    //    [Variable]
    //    public static string HousesDoorsFile = "HousesDoors.xml";


    //    protected override List<short> InternalGetOne(int id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + HousesDoorsFile))
    //        {
    //            var doors = Serializer.Deserialize<List<HouseDoors>>(sr.BaseStream).FirstOrDefault(h => h.HouseId == id);

    //            return doors != null ? doors.Doors : null;
    //        }
    //    }

    //    protected override Dictionary<int, List<short>> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + HousesDoorsFile))
    //        {
    //            return Serializer.Deserialize<List<HouseDoors>>(sr.BaseStream).ToDictionary(h=>h.HouseId,h=> h.Doors);
    //        }
    //    }
    //}
}