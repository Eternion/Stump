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
using Stump.BaseCore.Framework.Attributes;

namespace Stump.Server.BaseServer.Commands
{
    public abstract class CommandBase : ExecuterBase
    {
        [Variable]
        public static bool IgnoreCommandCase = true;

        protected CommandBase()
        {
            SubCommands = new List<SubCommand>();
        }

        public List<SubCommand> SubCommands
        {
            get;
            internal set;
        }

        /// <summary>
        ///   Try to get a SubCommand with its name.
        /// </summary>
        /// <param name = "subcmd">requested subcommand name</param>
        /// <param name = "result">Out the requested subcommand</param>
        /// <returns>if the requested subcommand exists.</returns>
        public bool TryGetSubCommand(string subcmd, out SubCommand result)
        {
            foreach (SubCommand sub in from sub in SubCommands
                                       from subalias in sub.Aliases
                                       where subalias == subcmd
                                       select sub)
            {
                result = sub;
                return true;
            }

            result = null;
            return false;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}