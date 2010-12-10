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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Chat
{
    public enum ChannelId
    {
        General = 0,
        Team = 1,
        Guild = 2,
        Alignment = 3,
        Group = 4,
        Trade = 5,
        Recruitment = 6,
        Newbies = 7,
        Administrators = 8,
        Private = 9,
        Information = 10,
        Fight = 11,
        End
    }

    public class Channel : IEntityContainer
    {
        private ConcurrentDictionary<long, Entity> m_entities;

        public Channel(ChannelId chanId)
        {
            Id = chanId;
            Name = Enum.GetName(typeof (ChannelId), chanId);
            m_entities = new ConcurrentDictionary<long, Entity>();
        }

        #region IEntityContainer Members

        public IEnumerable<IEntity> FindAll()
        {
            return m_entities.Values;
        }

        public IEntity Get(long id)
        {
            return m_entities[id];
        }

        #endregion

        #region Properties

        public ChannelId Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ConcurrentDictionary<long, Entity> Entities
        {
            get { return m_entities; }
            set { m_entities = value; }
        }

        #endregion
    }
}