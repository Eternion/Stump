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

namespace Stump.Database.Data.Effects
{
    [Serializable]
    [ActiveRecord("effects")]
    [AttributeAssociatedFile("Effects")]
    public sealed class EffectRecord : DataBaseRecord<EffectRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
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

        [Property("IconId")]
        public uint IconId
        {
            get;
            set;
        }

        [Property("Characteristic")]
        public int Characteristic
        {
            get;
            set;
        }

        [Property("Category")]
        public uint Category
        {
            get;
            set;
        }

        [Property("Operator")]
        public string Operator
        {
            get;
            set;
        }

        [Property("ShowInToolTip")]
        public bool ShowInToolTip
        {
            get;
            set;
        }

        [Property("UseDice")]
        public bool UseDice
        {
            get;
            set;
        }

        [Property("ForceMinMax")]
        public bool ForceMinMax
        {
            get;
            set;
        }

        [Property("ShowInSet")]
        public bool ShowInSet
        {
            get;
            set;
        }

        [Property("BonusType")]
        public int BonusType
        {
            get;
            set;
        }
    }
}