#region License GNU GPL
// ChatEntry.cs
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
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Social
{
    public class ChatEntry
    {
        public ChatEntry(string message, ChatActivableChannelsEnum channel, DateTime date)
        {
            Message = message;
            Channel = channel;
            Date = date;
        }

        public string Message
        {
            get;
            set;
        }

        public ChatActivableChannelsEnum Channel
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public Character Destinatory
        {
            get;
            set;
        }
    }
}