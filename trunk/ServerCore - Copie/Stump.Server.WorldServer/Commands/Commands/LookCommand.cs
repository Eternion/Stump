
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
            Parameters = new List<IParameter>
                {
                    new ParameterDefinition<Character>("target", "t", "Target who will have another look",
                                                    converter: ParametersConverter.CharacterConverter),
                    new ParameterDefinition<string>("look", "l", "The new look for the target")
                };
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = trigger.Get<Character>("target");

            target.Look = new ExtendedLook(trigger.Get<string>("look").ToEntityLook());

            target.Map.Do(character =>
                                                          ContextHandler.SendGameContextRefreshEntityLookMessage(
                                                              character.Client, target));
        }
    }
}