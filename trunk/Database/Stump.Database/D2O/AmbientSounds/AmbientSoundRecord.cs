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

namespace Stump.Database.D2O
{
    [Serializable]
    [ActiveRecord("ambient_sounds")]
    public sealed class AmbientSoundRecord : D2OBaseRecord<AmbientSoundRecord>
    {

        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("Volume")]
        public uint Volume
        {
            get; 
            set;
        }

        [Property("CriterionId")]
        public int CriterionId
        {
            get;
            set;
        }

        [Property("SilenceMin")]
        public uint SilenceMin
        {
            get;
            set;
        }

        [Property("SilenceMax")]
        public uint SilenceMax
        {
            get;
            set;
        }

        [Property("Channel")]
        public int Channel
        {
            get;
            set;
        }
    }
}