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

namespace Stump.Database.Data.Notifications
{
    [Serializable]
    [ActiveRecord("notifications")]
    [AttributeAssociatedFile("Notifications")]
    public sealed class NotificationRecord : DataBaseRecord<NotificationRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("TitleId")]
        public uint TitleId
        {
            get;
            set;
        }

        [Property("MessageId")]
        public uint MessageId
        {
            get;
            set;
        }

        [Property("IconId")]
        public int IconId
        {
            get;
            set;
        }

        [Property("TypeId")]
        public int TypeId
        {
            get;
            set;
        }

        [Property("NotificationTrigger")]
        public string Trigger
        {
            get;
            set;
        }
    }
}