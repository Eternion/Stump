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
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Npcs
{
    public class NpcDialogQuestion
    {
        public uint Id
        {
            get;
            private set;
        }

        public string[] Parameters
        {
            get;
            private set;
        }

        public Dictionary<uint, NpcDialogReply> Replies
        {
            get;
            private set;
        }

        public void CallReply(uint id, NpcSpawn npc, Character dialoger)
        {
            if (!Replies.ContainsKey(id))
                throw new ArgumentException("Reply with id '" + id + "' doesn't exist");

            Replies[id].CallAction(npc, dialoger);
        }
    }
}