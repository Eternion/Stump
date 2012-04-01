using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game;

namespace Stump.Plugins.DebugPlugin.Commands
{
    public class StartCommand : CommandBase
    {
        public StartCommand()
        {
            Aliases = new[] { "start" };
            RequiredRole = RoleEnum.Player;
            Description = "Teleport to the start map";
        }

        public override void Execute(TriggerBase trigger)
        {
            if (!(trigger is GameTrigger))
                return;

            var character = ( trigger as GameTrigger ).Character;

            var map = World.Instance.GetMap(character.Breed.StartMap);
            character.Teleport(character.Breed.GetStartPosition());

            trigger.Reply("Teleported to the start map");
        }
    }
}