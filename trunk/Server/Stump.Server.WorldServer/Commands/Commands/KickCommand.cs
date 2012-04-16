using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
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
            target.Client.Disconnect();
        }
    }
}