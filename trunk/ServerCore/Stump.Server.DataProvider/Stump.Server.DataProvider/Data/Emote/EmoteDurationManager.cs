// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Emote
{
    //public class EmoteDurationManager : DataManager<EmotesEnum, uint>
    //{
    //    /// <summary>
    //    ///   Name of EmoteDuration file
    //    /// </summary>
    //    [Variable]
    //    public static string EmotesFile = "EmotesDuration.xml";

    //    protected override uint GetData(EmotesEnum id)
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + EmotesFile))
    //        {
    //            var emote = Serializer.Deserialize<List<EmoteDuration>>(sr.BaseStream).FirstOrDefault(element => element.Id == id);

    //            return emote != null ? emote.Duration : 0;
    //        }
    //    }

    //    protected override Dictionary<EmotesEnum, uint> GetAllData()
    //    {
    //        using (var sr = new StreamReader(Settings.StaticPath + EmotesFile))
    //        {
    //            return Serializer.Deserialize<List<EmoteDuration>>(sr.BaseStream).ToDictionary(e=>e.Id, e=>e.Duration);
    //        }
    //    }
    //}
}