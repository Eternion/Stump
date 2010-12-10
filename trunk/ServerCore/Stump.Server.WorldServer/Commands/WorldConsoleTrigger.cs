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
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.WorldServer.Commands
{
    public class WorldConsoleTrigger : TriggerBase
    {
        public WorldConsoleTrigger(StringStream args)
            : base(args, RoleEnum.Administrator)
        {
        }

        public override void Reply(string text)
        {
            string name = GetBindedCommandName();

            if (name != "")
                Console.WriteLine(GetBindedCommandName() + " : " + text);
            else
                Console.WriteLine(text);
        }
    }
}