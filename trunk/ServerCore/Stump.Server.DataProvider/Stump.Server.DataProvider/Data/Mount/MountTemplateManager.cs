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
using System.IO;
using System.Linq;
using ProtoBuf;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Mount
{
    public class MountTemplateManager : DataManager<uint, MountTemplate>
    {

        /// <summary>
        ///   Name of Mount template file
        /// </summary>
        [Variable]
        public static string MountFile = "MountsTemplate.xml";

        protected override MountTemplate InternalGetOne(uint id)
        {
            using (var sr = new StreamReader(Settings.StaticPath + MountFile))
            {
                var mounts = Serializer.Deserialize<List<MountTemplate>>(sr.BaseStream);

                mounts[(int) id].Look = mounts[(int) id].LookStr.ToEntityLook();

                return mounts[(int)id];
            }
        }

        protected override Dictionary<uint, MountTemplate> InternalGetAll()
        {
            using (var sr = new StreamReader(Settings.StaticPath + MountFile))
            {
                var mounts = Serializer.Deserialize<List<MountTemplate>>(sr.BaseStream);

                foreach (var mount in mounts)
                {
                    mount.Look = mount.LookStr.ToEntityLook();
                }
                return mounts.ToDictionary(m => m.MountId);
            }
        }
    }
}