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
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Xml;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.Emote
{
    public static class EmoteDataProvider
    {
        /// <summary>
        ///   Name of Emote file
        /// </summary>
        [Variable] 
        public static string EmotesFile = "Emotes/EmotesDuration.xml";

        private static Dictionary<EmotesEnum, uint> m_emotesDuration;


        [StageStep(Stages.One, "Loaded Emotes durations")]
        public static void LoadEmotesDuration()
        {
            var list = XmlUtils.Deserialize<List<EmoteDuration>>(Settings.StaticPath + EmotesFile);

            m_emotesDuration = new Dictionary<EmotesEnum, uint>(list.Count);
            foreach (var element in list)
                m_emotesDuration.Add(element.Id, element.Duration);
        }


        public static uint GetEmoteDuration(EmotesEnum id)
        {
            if (m_emotesDuration.ContainsKey(id))
                return m_emotesDuration[id];
            return 0;
        }

        public static uint GetEmoteDuration(uint id)
        {
            return GetEmoteDuration((EmotesEnum) id);
        }
    }
}