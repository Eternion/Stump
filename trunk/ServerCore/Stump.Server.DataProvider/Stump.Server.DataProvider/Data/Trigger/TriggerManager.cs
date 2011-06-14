
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Trigger
{
    //public class TriggerManager : DataManager<int, List<TriggerTemplate>>
    //{
    //    /// <summary>
    //    ///   Name of Trigger file
    //    /// </summary>
    //    [Variable]
    //    public static string TriggerFile = "Triggers.xml";

    //    protected override List<TriggerTemplate> InternalGetOne(int id)
    //    {
    //        return null;
    //    }

    //    protected override Dictionary<int, List<TriggerTemplate>> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + TriggerFile))
    //        {
    //            return
    //                Serializer.Deserialize<List<TriggerTemplate>>(sr.BaseStream).GroupBy(t => t.MapId).
    //                ToDictionary(g=>(int)g.First().MapId,  g => g.ToList());
    //        }
    //    }
   // }
}