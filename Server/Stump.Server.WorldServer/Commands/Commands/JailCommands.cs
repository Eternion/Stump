using System;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class JailCommand : TargetCommand
    {
        public JailCommand()
        {
            Aliases = new[] {"jail"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Jail a character";
            AddTargetParameter();
            AddParameter("time", "time", "Jail duration (in minutes)", 30);
            AddParameter("reason", "r", "Reason of jail", "No reason");
        }

        public override void Execute(TriggerBase trigger)
        {
            var reason = trigger.Get<string>("reason");

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            foreach (var target in GetTargets(trigger))
            {
                var message = new BanAccountMessage
                {
                    AccountId = target.Account.Id,
                    BanReason = reason,
                };

                var source = trigger.GetSource() as WorldClient;
                if (source != null)
                    message.BannerAccountId = source.Account.Id;

                if (trigger.IsArgumentDefined("time"))
                {
                    var time = trigger.Get<int>("time");
                    message.BanEndDate = DateTime.Now + TimeSpan.FromMinutes(time);
                }
                else if (trigger.IsArgumentDefined("life"))
                    message.BanEndDate = null;
                else
                {
                    trigger.ReplyError("No ban duration given");
                    return;
                }

                target.TeleportToJail();
                target.Account.IsJailed = true;
                message.Jailed = true;

                IPCAccessor.Instance.SendRequest(message,
                    ok =>
                        trigger.Reply("Account {0} jailed for {1} minutes. Reason : {2}", target.Account.Login,
                            trigger.Get<int>("time"), reason),
                    error => trigger.ReplyError("Account {0} not jailed : {1}", target.Account.Login, error.Message));
            }
        }
    }

    public class UnJailCommand : TargetCommand
    {
        public UnJailCommand()
        {
            Aliases = new[] {"unjail"};
            RequiredRole = RoleEnum.Moderator;
            Description = "Unjail a player";
            AddTargetParameter();
        }
        public override void Execute(TriggerBase trigger)
        {
            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            foreach (var target in GetTargets(trigger))
            {
                var target1 = target;
                IPCAccessor.Instance.SendRequest(new UnBanAccountMessage(target.Account.Login),
                    ok => trigger.Reply("Account {0} unjailed", target1.Account.Login),
                    error =>
                        trigger.ReplyError("Account {0} not unjailed : {1}", target1.Account.Login, error.Message));

                if (!target.Account.IsJailed)
                    continue;

                target.Account.IsJailed = false;
                target.Account.BanEndDate = null;

                var target2 = target;
                target.Area.ExecuteInContext(() => target2.Teleport(target2.Breed.GetStartPosition()));

                target.SendServerMessage("Vous avez été libéré de prison.", Color.Red);
            }
        }
    }
}