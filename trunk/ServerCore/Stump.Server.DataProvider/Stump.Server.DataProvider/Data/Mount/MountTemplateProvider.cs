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
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ProtoBuf;
using Stump.BaseCore.Framework.Attributes;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.DataProvider.Core;
using Stump.Server.WorldServer.Data.House;

namespace Stump.Server.DataProvider.Data.Mount
{
    public class MountTemplateProvider : DataProvider<uint, MountTemplate>
    {

        /// <summary>
        ///   Name of Mount template file
        /// </summary>
        [Variable]
        public static string MountFile = "Mount/MountsTemplate.xml";

        protected override MountTemplate GetData(uint id)
        {
            var mountData = D2OLoader.LoadData<DofusProtocol.D2oClasses.Mount>().FirstOrDefault(m => m.id == id);

            using (var sr = new StreamReader(Settings.StaticPath + MountFile))
            {
                var mounts = Serializer.Deserialize<List<MountTemplate>>(sr.BaseStream);

                mounts[(int)id].Look = mountData.look.ToEntityLook();

                return mounts[(int)id];
            }
        }

        protected override Dictionary<uint, MountTemplate> GetAllData()
        {
            var mountsData = D2OLoader.LoadData<DofusProtocol.D2oClasses.Mount>().ToDictionary(m => m.id);

            using (var sr = new StreamReader(Settings.StaticPath + MountFile))
            {
                var mounts = Serializer.Deserialize<List<MountTemplate>>(sr.BaseStream);

                foreach (var mount in mounts)
                {
                    if (!mountsData.ContainsKey(mount.MountId))
                        throw new Exception("Can't find the mount");

                    mount.Look = mountsData[mount.MountId].look.ToEntityLook();
                }
                return mounts.ToDictionary(m => m.MountId);
            }
        }
    }
}