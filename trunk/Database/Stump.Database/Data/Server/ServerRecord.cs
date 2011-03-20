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
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.Data.Server
{
    [Serializable]
    [ActiveRecord("servers")]
    [AttributeAssociatedFile("Servers")]
    public sealed class ServerRecord : DataBaseRecord<ServerRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
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

        [Property("CommentId")]
        public uint CommentId
        {
            get;
            set;
        }

        [Property("OpeningDate")]
        public double OpeningDate
        {
            get;
            set;
        }

        [Property("Language")]
        public string Language
        {
            get;
            set;
        }

        [Property("PopulationId")]
        public int PopulationId
        {
            get;
            set;
        }

        [Property("GameTypeId")]
        public uint GameTypeId
        {
            get;
            set;
        }

        [Property("ComunityId")]
        public int ComunityId
        {
            get;
            set;
        }

        [Property("RestrictedToLanguages", ColumnType = "Serializable")]
        public List<string> RestrictedToLanguages
        {
            get;
            set;
        }
    }
}