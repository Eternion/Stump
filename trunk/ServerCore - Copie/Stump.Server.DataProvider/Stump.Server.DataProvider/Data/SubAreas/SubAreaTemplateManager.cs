
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.SubAreas
{
    //public class SubAreaTemplateManager : DataManager<int,SubAreaTemplate>
    //{
    //    /// <summary>
    //    ///   Name of SuperArea templates file
    //    /// </summary>
    //    [Variable]
    //    public static string SubAreaFile = "SubAreaTemplates.xml";

    //    protected override SubAreaTemplate InternalGetOne(int id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + SubAreaFile))
    //        {
    //            var superAreas = Serializer.Deserialize<List<SubAreaTemplate>>(sr.BaseStream);

    //            return superAreas[id];
    //        }
    //    }

    //    protected override Dictionary<int, SubAreaTemplate> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + SubAreaFile))
    //        {
    //            return Serializer.Deserialize<List<SubAreaTemplate>>(sr.BaseStream).ToDictionary(s => s.Id);
    //        }
    //    }
    //}
}