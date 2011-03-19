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
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.Data.Communication
{
    [Serializable]
    [ActiveRecord("caht_channels")]
    [AttributeAssociatedFile("ChatChannels")]
    public sealed class ChatChannelRecord : DataBaseRecord<ChatChannelRecord>
    {

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        [Property("DescriptionId")]
        public uint DescriptionId
        {
            get;
            set;
        }

        [Property("Shortcut")]
        public string Shortcut
        {
            get;
            set;
        }

        [Property("ShortcutKey")]
        public string ShortcutKey
        {
            get;
            set;
        }

        [Property("IsPrivate")]
        public bool IsPrivate
        {
            get;
            set;
        }

        [Property("AllowObjects")]
        public bool AllowObjects
        {
            get;
            set;
        }
    }
}