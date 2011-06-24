
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Mount
{
    //public class MountTemplateManager : DataManager<uint, MountTemplate>
    //{

    //    /// <summary>
    //    ///   Name of Mount template file
    //    /// </summary>
    //    [Variable]
    //    public static string MountFile = "MountsTemplate.xml";

    //    protected override MountTemplate InternalGetOne(uint id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + MountFile))
    //        {
    //            var mounts = Serializer.Deserialize<List<MountTemplate>>(sr.BaseStream);

    //            mounts[(int) id].Look = mounts[(int) id].LookStr.ToEntityLook();

    //            return mounts[(int)id];
    //        }
    //    }

    //    protected override Dictionary<uint, MountTemplate> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + MountFile))
    //        {
    //            var mounts = Serializer.Deserialize<List<MountTemplate>>(sr.BaseStream);

    //            foreach (var mount in mounts)
    //            {
    //                mount.Look = mount.LookStr.ToEntityLook();
    //            }
    //            return mounts.ToDictionary(m => m.MountId);
    //        }
    //    }
    //}
}