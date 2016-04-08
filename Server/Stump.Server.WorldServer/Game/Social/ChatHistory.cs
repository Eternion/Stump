#region License GNU GPL
// ChatHistory.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Social
{
    public class ChatHistory
    {
        [Variable] public static int MaxChatEntries = 50;

        private readonly List<ChatEntry> m_entries = new List<ChatEntry>();

        public ChatHistory(Character character)
        {
            Character = character;
        }

        public Character Character
        {
            get;
            private set;
        }

        public ReadOnlyCollection<ChatEntry> Entries
        {
            get { return m_entries.AsReadOnly(); }
        }

        public bool RegisterAndCheckFlood(ChatEntry entry)
        {
            m_entries.Insert(0, entry);

            while (m_entries.Count >= MaxChatEntries)
            {
                m_entries.Remove(m_entries.Last());
            }

            if (Character.IsGameMaster())
                return true;

            if (m_entries.Count > 1 &&
                ( m_entries[0].Date - m_entries[1].Date )
                    .TotalMilliseconds < ChatManager.AntiFloodTimeBetweenTwoMessages)
            {
                return false;
            }

            if (m_entries.Count > 1 && ChatManager.IsGlobalChannel(entry.Channel))
            {
                var entryIndex = m_entries.FindIndex(1, x => x.Channel == entry.Channel);

                if (entryIndex >= 0 &&
                    (entry.Date - m_entries[entryIndex].Date).TotalSeconds < ChatManager.AntiFloodTimeBetweenTwoGlobalMessages)
                {
                    // Ce canal est restreint pour améliorer sa lisibilité. Vous pourrez envoyer un nouveau message dans %1 secondes.
                    // Ceci ne vous autorise cependant pas pour autant à surcharger ce canal.
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 115,
                                                     ChatManager.AntiFloodTimeBetweenTwoGlobalMessages - (int)( entry.Date - m_entries[entryIndex].Date ).TotalSeconds);

                    m_entries.Remove(entry);

                    return false;
                }
            }

            if (m_entries.Count < ChatManager.AntiFloodAllowedMessages ||
                !m_entries.Take(ChatManager.AntiFloodAllowedMessages)
                    .All(x => (DateTime.Now - x.Date).TotalSeconds < ChatManager.AntiFloodAllowedMessagesResetTime))
                return true;

            Character.Mute(TimeSpan.FromSeconds(ChatManager.AntiFloodMuteTime));
            return false;
        }
    }
}