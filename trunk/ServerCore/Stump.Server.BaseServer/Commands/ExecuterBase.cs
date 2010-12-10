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
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands
{
    /// <summary>
    ///   Only used to make SubCommand and CommandBase shorter
    /// </summary>
    public abstract class ExecuterBase
    {
        public string[] Aliases
        {
            get;
            protected set;
        }

        public string Usage
        {
            get;
            protected set;
        }

        public string Description
        {
            get;
            protected set;
        }

        public RoleEnum RequiredRole
        {
            get;
            protected set;
        }

        public IEnumerable<ICommandParameter> Parameters
        {
            get;
            protected set;
        }

        public string GetSafeUsage()
        {
            if (string.IsNullOrEmpty(Usage))
            {
                if (Parameters == null)
                    return "";

                return string.Join(" ", from entry in Parameters
                                        select
                                            entry.GetUsage());
            }
            else return Usage;
        }

        public abstract void Execute(TriggerBase trigger);
    }
}