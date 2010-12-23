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
using System.Linq;
using Stump.BaseCore.Framework.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Actions
{
    public static class ActionManager
    {
        private static readonly Dictionary<ActionsEnum, Delegate> ActionsDelegate = new Dictionary<ActionsEnum, Delegate>();

        public static void Initialize()
        {
            var actions =
                typeof(ActionManager).Assembly.GetTypes().Where(entry => !entry.IsAbstract && entry.IsSubclassOf(typeof(ActionBase)));

            foreach (var action in actions)
            {
                var attribute = action.GetCustomAttributes(false).OfType<ActionTypeAttribute>().SingleOrDefault();

                if (attribute == null)
                {
                    throw new Exception("Attribute 'ActionTypeAttribute' is not present on action '" + action.Name + "'");
                }

                var constructors = action.GetConstructors();

                if (constructors.Count() != 1)
                {
                    throw new Exception("Correct constructor of action '" + action.Name + "' cannot be found");
                }

                ActionsDelegate.Add(attribute.Action, constructors.First().CreateDelegate(constructors.First().GetFuncType()));
            }
        }

        public static void ExecuteAction(ActionsEnum actionType, params object[] args)
        {
            if (!ActionsDelegate.ContainsKey(actionType))
                throw new NotImplementedException("Action '" + actionType + "' is not implemented");

            var action = (ActionBase)ActionsDelegate[actionType].DynamicInvoke(args);

            action.Execute();
        }
    }
}