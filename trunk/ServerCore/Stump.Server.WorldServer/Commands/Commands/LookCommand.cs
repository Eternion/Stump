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
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Classes.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Commands
{
    public class LookCommand : WorldCommand
    {
        public LookCommand()
        {
            Aliases = new[] {"look"};
            RequiredRole = RoleEnum.GameMaster_Padawan;
            Description = "Change the look of the target";
            Parameters = new List<ICommandParameter>
                {
                    new CommandParameter<Character>("target", "t", "Target who will have another look",
                                                    converter: ParametersConverter.CharacterConverter),
                    new CommandParameter<string>("look", "l", "The new look for the target")
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = trigger.GetArgument<Character>("target");

            target.Look = new ExtendedLook(trigger.GetArgument<string>("look").ToEntityLook());

            target.Map.CallOnAllCharactersWithoutFighters(character =>
                                                          ContextHandler.SendGameContextRefreshEntityLookMessage(
                                                              character.Client, target));
        }
    }
}