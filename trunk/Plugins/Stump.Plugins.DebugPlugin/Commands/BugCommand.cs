using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;

namespace Stump.Plugins.DebugPlugin.Commands
{
    public class BugCommand : CommandBase
    {
        public BugCommand()
        {
            Aliases = new[] { "bug" };
            RequiredRole = RoleEnum.Player;
            Usage = "bug {bug description}";
            Description = "Report a bug";
            AddParameter("target", "t", "Target to teleport",
                                            converter: ParametersConverter.CharacterConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            var report = trigger.Args.NextWords();
            var target = trigger.Get<Character>("target");

            target.LevelUp(1);
            trigger.Reply("gg !!");

            // todo : report
        }
    }
}