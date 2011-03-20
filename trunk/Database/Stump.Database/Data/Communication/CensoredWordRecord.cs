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
    [ActiveRecord("censored_words")]
    [AttributeAssociatedFile("CensoredWords")]
    public sealed class CensoredWordRecord : DataBaseRecord<CensoredWordRecord>
    {

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("ListId")]
        public uint ListId
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

        [Property("Word")]
        public string Word
        {
            get;
            set;
        }

        [Property("DeepLooking")]
        public string DeepLooking
        {
            get;
            set;
        }
    }
}