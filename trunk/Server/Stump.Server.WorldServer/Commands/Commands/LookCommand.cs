using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class LookCommand : CommandBase
    {
        public LookCommand()
        {
            Aliases = new[] {"look"};
            RequiredRole = RoleEnum.GameMaster_Padawan;
            Description = "Change the look of the target";
            AddParameter("target", "t", "Target who will have another look",
                         converter: ParametersConverter.CharacterConverter);
            AddParameter<string>("look", "l", "The new look for the target");
            AddParameter("demorph", "demorph", "Regive the base skin to the target", false, true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = trigger.Get<Character>("target");

            if (trigger.Get<bool>("demorph"))
            {
                target.CustomLookActivated = false;
                return;
            }

            target.CustomLook = trigger.Get<string>("look").ToEntityLook();
            target.CustomLookActivated = true;

            target.Map.ForEach(character =>
                                ContextHandler.SendGameContextRefreshEntityLookMessage(
                                    character.Client, target));
        }
    }
}