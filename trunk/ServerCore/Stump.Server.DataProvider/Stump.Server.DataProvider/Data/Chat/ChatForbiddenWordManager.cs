
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
    //public class ChatForbiddenWordManager : DataManager<uint,string>
    //{

        //protected override string InternalGetOne(uint id)
        //{
        //    var cw = D2OLoader.LoadData<CensoredWord>((int)id);
        //    return cw != null ? cw.word : "";
        //}

        //protected override Dictionary<uint, string> InternalGetAll()
        //{
        //    return D2OLoader.LoadData<CensoredWord>().ToDictionary(w => w.id, w => w.word);
        //}

        ///// <summary>
        ///// Only with pre-loaded
        ///// </summary>
        ///// <param name="word"></param>
        ///// <returns></returns>
        //public bool IsCensored(string word)
        //{
        //    return PreLoadData.ContainsValue(word);
        //}
   // }
}




