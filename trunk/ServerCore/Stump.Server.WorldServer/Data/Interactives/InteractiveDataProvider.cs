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
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;

namespace Stump.Server.WorldServer.Data.Job
{
    public static class InteractiveDataProvider
    {
        private static Dictionary<int, int> m_interactives;


        [StageStep(Stages.One, "Loaded Interactive Actions")]
        public static void LoadJobsTemplates()
        {
            var interactives = DataLoader.LoadData<DofusProtocol.D2oClasses.Interactive>();

            m_interactives = new Dictionary<int, int>(interactives.Count());
            foreach (var interactive in interactives)
                m_interactives.Add(interactive.id, interactive.actionId);
        }


        public static int GetActionId(int elementId)
        {
            if (m_interactives.ContainsKey(elementId))
                return m_interactives[elementId];
            return 0;
        }
    }
}