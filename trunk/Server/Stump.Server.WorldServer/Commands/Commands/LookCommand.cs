using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.Look;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class LookCommand : TargetCommand
    {
        public LookCommand()
        {
            Aliases = new[] {"look"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Change the look of the target";
            AddParameter<string>("look", "l", "The new look for the target", isOptional:true);
            AddTargetParameter(true);
            AddParameter<bool>("demorph", "demorph", "Regive the base skin to the target", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var source = trigger.GetSource() as WorldClient;

            if (source.Account.Role <= RoleEnum.GameMaster_Padawan)
            {
                target.CustomLook = ActorLook.Parse("{705}");
                target.CustomLookActivated = true;

                target.RefreshActor();
                return;
            }

            if (trigger.IsArgumentDefined("demorph"))
            {
                target.CustomLookActivated = false;
                target.CustomLook = null;
                trigger.Reply("Demorphed");

                target.Map.Area.ExecuteInContext(() =>
                    target.Map.Refresh(target));
                return;
            }

            if (!trigger.IsArgumentDefined("look"))
            {
                trigger.ReplyError("Look not defined");
                return;
            }

            target.CustomLook = ActorLook.Parse(trigger.Get<string>("look"));
            target.CustomLookActivated = true;

            target.RefreshActor();
        }
    }
}