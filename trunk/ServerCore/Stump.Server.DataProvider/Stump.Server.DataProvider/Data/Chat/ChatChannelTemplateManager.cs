
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Chat
{
    //public class ChatChannelTemplateManager : DataManager<ChannelId, ChannelTemplate>
    //{
    //    /// <summary>
    //    ///   Name of Channels file
    //    /// </summary>
    //    [Variable]
    //    public static string ChannelFile = "Chat/Channels.xml";

    //    protected override ChannelTemplate InternalGetOne(ChannelId id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + ChannelFile))
    //        {
    //            return Serializer.Deserialize<List<ChannelTemplate>>(sr.BaseStream).FirstOrDefault(c => c.Id == id);
    //        }
    //    }

    //    protected override Dictionary<ChannelId, ChannelTemplate> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + ChannelFile))
    //        {
    //            return Serializer.Deserialize<List<ChannelTemplate>>(sr.BaseStream).ToDictionary(c => c.Id);
    //        }
    //    }
    //}
}




