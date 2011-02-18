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
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.DataProvider.Data.Job
{
    public class JobSkill
    {
        public int SkillId { get; set; }

        public JobTemplate Job { get; set; }

        public int InteractiveId { get; set; }

        public bool IsForgemagus { get; set; }

        public bool IsRepair { get; set; }

        public bool AvailableInHouse { get; set; }

        public ItemTypeEnum ModifiableItemType { get; set; }

        public int GatheredRessource { get; set; }

        public List<int> CraftableItemIds { get; set; }
    }
}