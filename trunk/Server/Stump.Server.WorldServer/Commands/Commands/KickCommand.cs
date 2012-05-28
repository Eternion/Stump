using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class KickCommand : TargetCommand
    {
        public KickCommand()
        {
            Aliases = new[] { "kick" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Kick a player";

            AddTargetParameter();
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var kicker = (trigger is GameTrigger) ? (trigger as GameTrigger).Character.Name : "Server";

            target.SendSystemMessage(18, true, kicker, string.Empty); // you were kicked by %1
            target.Client.Disconnect();

            trigger.Reply("You have kicked {0}", target.Name);
        }
    }
}