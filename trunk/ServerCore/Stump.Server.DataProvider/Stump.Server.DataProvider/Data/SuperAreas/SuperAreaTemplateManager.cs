
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.Server.DataProvider.Core;
using Stump.Server.DataProvider.Data.Mount;
using Stump.Server.DataProvider.Data.Recipe;

namespace Stump.Server.DataProvider.Data.SuperAreas
{
    //public class SuperAreaTemplateManager : DataManager<int,SuperAreaTemplate>
    //{
    //    /// <summary>
    //    ///   Name of SuperArea templates file
    //    /// </summary>
    //    [Variable]
    //    public static string SuperAreaFile = "SuperAreaTemplates.xml";

    //    protected override SuperAreaTemplate InternalGetOne(int id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + SuperAreaFile))
    //        {
    //            var superAreas = Serializer.Deserialize<List<SuperAreaTemplate>>(sr.BaseStream);

    //            return superAreas[id];
    //        }
    //    }

    //    protected override Dictionary<int, SuperAreaTemplate> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + SuperAreaFile))
    //        {
    //            return Serializer.Deserialize<List<SuperAreaTemplate>>(sr.BaseStream).ToDictionary(s => s.Id);
    //        }
    //    }
    //}
}