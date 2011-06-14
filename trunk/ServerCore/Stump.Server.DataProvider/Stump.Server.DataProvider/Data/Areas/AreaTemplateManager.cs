
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Areas
{
    //public class AreaTemplateManager : DataManager<int,AreaTemplate>
    //{
    //    /// <summary>
    //    ///   Name of Area templates file
    //    /// </summary>
    //    [Variable]
    //    public static string AreaFile = "AreaTemplates.bin";

    //    protected override AreaTemplate InternalGetOne(int id)
    //    {
    //        using (var sr = new StreamReader(Path.Combine(Settings.StaticPath, AreaFile)))
    //        {
    //            var areas = Serializer.Deserialize<List<AreaTemplate>>(sr.BaseStream);

    //            return areas[id];
    //        }
    //    }

    //    protected override Dictionary<int, AreaTemplate> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Path.Combine(Settings.StaticPath, AreaFile)))
    //        {
    //            return Serializer.Deserialize<List<AreaTemplate>>(sr.BaseStream).ToDictionary(s => s.Id);
    //        }
    //    }

    //    protected override void FlushAdd(DataModification<int, AreaTemplate> modification)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void FlushRemove(DataModification<int, AreaTemplate> modification)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    protected override void FlushModify(DataModification<int, AreaTemplate> modification)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}