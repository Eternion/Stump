using System;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class MuteCommand : TargetCommand
    {
        public MuteCommand()
        {
            Aliases = new[] { "mute" };
            RequiredRole = RoleEnum.Moderator;
            AddTargetParameter();
            AddParameter("time", "time", "Mute for x minutes", 5);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var time = trigger.Get<int>("time") > 720 ? 720 : trigger.Get<int>("time");

            target.Mute(TimeSpan.FromMinutes(time), trigger.User as Character);
            trigger.Reply("{0} muted", target.Name);
        }
    }

    public class UnMuteCommand : TargetCommand
    {
        public UnMuteCommand()
        {
            Aliases = new[] { "unmute" };
            RequiredRole = RoleEnum.Moderator;
            AddTargetParameter();
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);

            target.UnMute(); 
            trigger.Reply("{0} unmuted", target.Name);
        }
    }

    public class MuteMapCommand : CommandBase
    {
        public MuteMapCommand()
        {
            Aliases = new[] { "mutemap" };
            RequiredRole = RoleEnum.Moderator;
        }

        public override void Execute(TriggerBase trigger)
        {
            var map = ((GameTrigger) trigger).Character.Map;
            var mute = map.ToggleMute();

            foreach (var character in map.GetAllCharacters())
            {
                character.SendServerMessage(mute ? "La map est maintenant réduite au silence !" : "La map n'est plus réduite au silence !", Color.Red);
            }
        }
    }
}