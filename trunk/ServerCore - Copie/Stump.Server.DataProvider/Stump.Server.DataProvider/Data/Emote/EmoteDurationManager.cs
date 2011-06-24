
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Emote
{
    ////public class EmoteDurationManager : DataManager<EmotesEnum, uint>
    ////{
    ////    /// <summary>
    ////    ///   Name of EmoteDuration file
    ////    /// </summary>
    ////    [Variable]
    ////    public static string EmotesFile = "EmotesDuration.xml";

    //    protected override uint InternalGetOne(EmotesEnum id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + EmotesFile))
    //        {
    //            var emote = Serializer.Deserialize<List<EmoteDuration>>(sr.BaseStream).FirstOrDefault(element => element.Id == id);

    ////            return emote != null ? emote.Duration : 0;
    ////        }
    ////    }

    //    protected override Dictionary<EmotesEnum, uint> InternalGetAll()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + EmotesFile))
    //        {
    //            return Serializer.Deserialize<List<EmoteDuration>>(sr.BaseStream).ToDictionary(e=>e.Id, e=>e.Duration);
    //        }
    //    }
  //  }
}