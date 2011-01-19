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
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.AuthServer.Managers
{
    public static class StartupActionManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static bool CreateStartupAction(StartupActionRecord startupAction)
        {
            if (StartupActionRecord.Exists(startupAction))
                return false;

            startupAction.CreateAndFlush();

            return true;
        }

        public static bool DeleteStartupAction(StartupActionRecord startupAction)
        {
            if (startupAction == null)
                return false;
            
            startupAction.DeleteAndFlush();

            return true;
        }

        public static bool AddStartupActionItem(StartupActionRecord startupAction, StartupActionItemRecord item)
        {
            if (startupAction.Items.Contains(item))
                return false;

            startupAction.Items.Add(item);

            return true;
        }

        public static bool DeleteStartupActionItem(StartupActionRecord startupAction, StartupActionItemRecord item)
        {
            if (!startupAction.Items.Contains(item))
                return false;

            startupAction.Items.Remove(item);

            return true;
        }

    }
}