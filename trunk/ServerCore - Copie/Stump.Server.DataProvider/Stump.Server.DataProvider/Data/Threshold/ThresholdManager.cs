
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Threshold
{
    //public abstract class ThresholdManager : DataManager<string, ThresholdDictionnary>
    //{
    //    /// <summary>
    //    ///   Name of the Thresholds file
    //    /// </summary>
    //    [Variable]
    //    public static string ThresholdsFile = "Thresholds.xml";

    //    protected override ThresholdDictionnary InternalGetOne(string id)
    //    {
    //        using (var reader = new StreamReader(Settings.StaticPath + ThresholdsFile))
    //        {
    //            var t = Serializer.Deserialize<List<ThresholdDictionnaryTemplate>>(reader.BaseStream).FirstOrDefault(  th => th.Name == id);

    //            return t == null ? null : new ThresholdDictionnary(t.Name, t.Thresholds.ToDictionary(th => th.Level, th => th.Value));
    //        }
    //    }

    //    protected override Dictionary<string, ThresholdDictionnary> InternalGetAll()
    //    {
    //        using (var reader = new StreamReader(Settings.StaticPath + ThresholdsFile))
    //        {
    //            return Serializer.Deserialize<List<ThresholdDictionnaryTemplate>>(reader.BaseStream).
    //                Select(t => new ThresholdDictionnary(t.Name, t.Thresholds.ToDictionary(t2 => t2.Level, t2 => t2.Value))).
    //                ToDictionary(t3 => t3.Name);
    //        }
    //    }
    //}
}