using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class KickCommand : CommandBase
    {
        public KickCommand()
        {
            Aliases = new[] { "kick" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Kick a player";

            AddParameter("target", "t", "Player to ban", converter: ParametersConverter.CharacterConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = trigger.Get<Character>("target");

            if (target == null)
            {
                trigger.ReplyError("Invalid target");
                return;
            }

            target.Client.Disconnect();
        }
    }
}