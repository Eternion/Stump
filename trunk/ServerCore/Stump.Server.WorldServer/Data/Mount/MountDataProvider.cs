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
using System.Linq;
using System.Collections.Generic;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.MountData
{
    public static class MountDataProvider
    {
        private static Dictionary<uint, EntityLook> m_mountsLook;

        private static List<RideFood> m_mountsFoods;

        [StageStep(Stages.One, "Loaded Mounts Looks")]
        public static void LoadMountsLook()
        {
            var mounts = DataLoader.LoadData<Mount>();

            m_mountsLook = new Dictionary<uint, EntityLook>(mounts.Count());
            foreach (var mount in mounts)
                m_mountsLook.Add(mount.id, mount.look.ToEntityLook());
        }

        [StageStep(Stages.One, "Loaded Mounts Food")]
        public static void LoadMountsFood()
        {
            m_mountsFoods = DataLoader.LoadData<RideFood>().ToList();
        }

        public static EntityLook GetMountLook(uint mountId)
        {
            if (m_mountsLook.ContainsKey(mountId))
                return m_mountsLook[mountId];
            return null;
        }

        public static bool IsRideFood(uint gid, uint typeId)
        {
            return m_mountsFoods.Where(rf => rf.gid == gid || rf.typeId == typeId).Count() != 0;
        }

    }
}