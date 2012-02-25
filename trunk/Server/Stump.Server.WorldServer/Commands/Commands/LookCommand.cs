using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context;

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
            AddParameter<string>("look", "l", "The new look for the target", isOptional:true);
            AddParameter<bool>("demorph", "demorph", "Regive the base skin to the target", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = trigger.Get<Character>("target");

            if (trigger.IsArgumentDefined("demorph"))
            {
                target.CustomLookActivated = false;
                target.CustomLook = null;
                target.Map.Refresh(target);
                return;
            }

            if (!trigger.IsArgumentDefined("look"))
            {
                trigger.ReplyError("Look not defined");
                return;
            }

            target.CustomLook = trigger.Get<string>("look").ToEntityLook();
            target.CustomLookActivated = true;

            target.Map.Refresh(target);
        }
    }
}