
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

            target.Map.Do(character =>
                                                          ContextHandler.SendGameContextRefreshEntityLookMessage(
                                                              character.Client, target));
        }
    }
}