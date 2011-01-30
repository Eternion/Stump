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
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Xml;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Data.Emote;

namespace Stump.Server.WorldServer.Data.Chat
{
    public static class ChatDataProvider
    {

        /// <summary>
        ///   Name of Channels file
        /// </summary>
        [Variable]
        public static string ChannelFile = "Chat/Channels.xml";

        private static List<string> m_censoredWords;

        private static Dictionary<int, ChannelTemplate> m_channelTemplates;


        [StageStep(Stages.One, "Loaded Channel Templates")]
        public static void LoadEmotesDuration()
        {
            var list = XmlUtils.Deserialize<List<ChannelTemplate>>(Settings.StaticPath + ChannelFile);

            m_emotesDuration = new Dictionary<, uint>(list.Count);
            foreach (var element in list)
                m_emotesDuration.Add(element.Id, element.Duration);
        }

        [StageStep(Stages.One, "Loaded Censored Words")]
        public static void LoadCensoredWords()
        {
             m_censoredWords = DataLoader.LoadData<CensoredWord>().Select(w => w.word).ToList();
        }


        public static bool IsCensored(string message)
        {
            return m_censoredWords.All(message.Contains);
        }

        public static ChannelTemplate GetChannelTemplate(int id)
        {
            if (m_channelTemplates.ContainsKey(id))
                return m_channelTemplates[id];
            return null;
        }
    }
}




