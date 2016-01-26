using System;
using System.Drawing;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class BanCommand : TargetCommand
    {
        public BanCommand()
        {
            Aliases = new[] { "ban" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Ban a player";

            AddTargetParameter();
            AddParameter<int>("time", "time", "Ban duration (in minutes)", isOptional: true);
            AddParameter<string>("reason", "r", "Reason of ban");
            AddParameter<bool>("life", "l", "Specify a life ban", isOptional: true);
            AddParameter<bool>("ip", "ip", "Also ban the ip", isOptional: true);
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
                    if (time > 60 * 24 && trigger.UserRole == RoleEnum.GameMaster_Padawan)
                        // max ban time for padawan == 24h
                        time = 60 * 24;
                    message.BanEndDate = DateTime.Now + TimeSpan.FromMinutes(time);
                }
                else if (trigger.IsArgumentDefined("life") && trigger.UserRole != RoleEnum.GameMaster_Padawan)
                    message.BanEndDate = null;
                else
                {
                    trigger.ReplyError("No ban duration given");
                    return;
                }

                message.Jailed = false;
                target.Client.Disconnect();

                IPCAccessor.Instance.SendRequest(message,
                    ok => trigger.Reply("Account {0} banned for {1} minutes. Reason : {2}", target.Account.Login,
                            trigger.Get<int>("time"), reason),
                    error => trigger.ReplyError("Account {0} not banned : {1}", target.Account.Login, error.Message));


                if (!trigger.IsArgumentDefined("ip"))
                    return;

                var banIPMessage = new BanIPMessage
                {
                    IPRange = target.Client.IP,
                    BanReason = reason,
                    BanEndDate = message.BanEndDate,
                    BannerAccountId = message.BannerAccountId
                };

                IPCAccessor.Instance.SendRequest(banIPMessage,
                    ok => trigger.Reply("IP {0} banned", target.Client.IP),
                    error => trigger.ReplyError("IP {0} not banned : {1}", target.Client.IP, error.Message));

            }
        }
    }

    public class BanIpCommand : CommandBase
    {
        public BanIpCommand()
        {
            Aliases = new[] { "banip" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Ban an ip";

            AddParameter<string>("ip", "ip", "The ip to ban");
            AddParameter<int>("time", "time", "Ban duration (in minutes)", isOptional: true);
            AddParameter("reason", "r", "Reason of ban", "No reason");
            AddParameter<bool>("life", "l", "Specify a life ban", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var ip = trigger.Get<string>("ip");
            var reason = trigger.Get<string>("reason");

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            try
            {
                IPAddressRange.Parse(ip);
            }
            catch
            {
                trigger.ReplyError("IP format '{0}' incorrect", ip);
                return;
            }

            var message = new BanIPMessage
            {
                IPRange = ip,
                BanReason = reason,
            };

            var source = trigger.GetSource() as WorldClient;
            if (source != null)
                message.BannerAccountId = source.Account.Id;

            if (trigger.IsArgumentDefined("time"))
                message.BanEndDate = DateTime.Now + TimeSpan.FromMinutes(trigger.Get<int>("time"));
            else if (trigger.IsArgumentDefined("life"))
                message.BanEndDate = null;
            else
            {
                trigger.ReplyError("No ban duration given");
                return;
            }

            IPCAccessor.Instance.SendRequest(message,
                ok => trigger.Reply("IP {0} banned", ip),
                error => trigger.ReplyError("IP {0} not banned : {1}", ip, error.Message));
        }
    }

    public class UnBanIPCommand : CommandBase
    {
        public UnBanIPCommand()
        {
            Aliases = new[] { "unbanip" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Unban an ip";

            AddParameter<string>("ip", "ip", "The ip to unban");
        }

        public override void Execute(TriggerBase trigger)
        {
            var ip = trigger.Get<string>("ip");

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            IPCAccessor.Instance.SendRequest(new UnBanIPMessage(ip),
                ok => trigger.Reply("IP {0} unbanned", ip),
                error => trigger.ReplyError("IP {0} not unbanned : {1}", ip, error.Message));
        }
    }

    public class BanHardwareIdCommand : CommandBase
    {
        public BanHardwareIdCommand()
        {
            Aliases = new[] { "hwban" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Ban an HardwareId";

            AddParameter<string>("hardwareId", "hw", "The hardwareId to ban");
            AddParameter<string>("reason", "r", "Reason of ban");
        }

        public override void Execute(TriggerBase trigger)
        {
            var hardwareId = trigger.Get<string>("hardwareId");
            var reason = trigger.Get<string>("reason");

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            var message = new BanHardwareIdMessage
            {
                HardwareId = hardwareId,
                BanReason = reason
            };

            var source = trigger.GetSource() as WorldClient;
            if (source != null)
                message.BannerAccountId = source.Account.Id;

            IPCAccessor.Instance.SendRequest(message,
                ok =>
                {
                    World.Instance.ForEachCharacter(x => x.Account.LastHardwareId == hardwareId, character => character.Client.Disconnect());
                    trigger.Reply("HardwareId {0} banned", hardwareId);
                },
                error => trigger.ReplyError("HardwareId {0} not banned : {1}", hardwareId, error.Message));
        }
    }

    public class UnBanHardwareIdCommand : CommandBase
    {
        public UnBanHardwareIdCommand()
        {
            Aliases = new[] { "hwunban" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Unban an HardwareId";

            AddParameter<string>("hardwareId", "hw", "The hardwareId to unban");
        }

        public override void Execute(TriggerBase trigger)
        {
            var hardwareId = trigger.Get<string>("hardwareId");

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            IPCAccessor.Instance.SendRequest(new UnBanHardwareIdMessage(hardwareId),
                ok => trigger.Reply("HardwareId {0} unbanned", hardwareId),
                error => trigger.ReplyError("HardwareId {0} not unbanned : {1}", hardwareId, error.Message));
        }
    }

    public class BanInfoCommand : CommandBase
    {
        public BanInfoCommand()
        {
            Aliases = new[] { "baninfo", "jailinfo" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Gives info about a ban";
            AddParameter<string>("player", "player", "Player name", isOptional: true);
            AddParameter<string>("account", "acc", "Account login", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var accDefined = trigger.IsArgumentDefined("account");
            var playerDefined = trigger.IsArgumentDefined("player");
            if (!(accDefined ^ playerDefined))
            {
                trigger.ReplyError("No parameter given");
                return;
            }

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }


            if (accDefined)
            {
                var message = new AccountRequestMessage { Login = trigger.Get<string>("account") };
                IPCAccessor.Instance.SendRequest<AccountAnswerMessage>(message, reply => OnReply(trigger, reply));
            }
            else
            {
                WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
                {
                    var character =
                        CharacterManager.Instance.GetCharacterByName(trigger.Get<string>("player"));

                    if (character == null)
                    {
                        trigger.ReplyError("Player {0} not found", trigger.Get<string>("player"));
                        return;
                    }

                    var message = new AccountRequestMessage() { CharacterId = character.Id };
                    IPCAccessor.Instance.SendRequest<AccountAnswerMessage>(message, reply => OnReply(trigger, reply),
                        error => trigger.ReplyError("Cannot get player {0} account : {1}", character.Name, error.Message));
                });
            }
        }

        private static void OnReply(TriggerBase trigger, AccountAnswerMessage reply)
        {
            trigger.Reply("Account : {0} ({1})", trigger.Bold(reply.Account.Login),
               trigger.Bold(reply.Account.Id));

            trigger.Reply("Banned : {0}", trigger.Bold(reply.Account.IsBanned));
            trigger.Reply("Jailed : {0}", trigger.Bold(reply.Account.IsJailed));
            trigger.Reply("Reason : {0}", trigger.Bold(reply.Account.BanReason));
            trigger.Reply("Until : {0}", trigger.Bold(reply.Account.BanEndDate));
        }
    }

    public class BanAccountCommand : CommandBase
    {
        public BanAccountCommand()
        {
            Aliases = new[] { "banacc" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Ban an account";

            AddParameter<string>("account", "account", "Account login");
            AddParameter<int>("time", "time", "Ban duration (in minutes)", isOptional: true);
            AddParameter("reason", "r", "Reason of ban", "No reason");
            AddParameter<bool>("life", "l", "Specify a life ban", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var accountName = trigger.Get<string>("account");
            var reason = trigger.Get<string>("reason");

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            var message = new BanAccountMessage
            {
                AccountName = accountName,
                BanReason = reason,
            };

            var source = trigger.GetSource() as WorldClient;
            if (source != null)
                message.BannerAccountId = source.Account.Id;

            if (trigger.IsArgumentDefined("time"))
                message.BanEndDate = DateTime.Now + TimeSpan.FromMinutes(trigger.Get<int>("time"));
            else if (trigger.IsArgumentDefined("life"))
                message.BanEndDate = null;
            else
            {
                trigger.ReplyError("No ban duration given");
                return;
            }

            IPCAccessor.Instance.SendRequest(message,
                ok => trigger.Reply("Account {0} banned", accountName),
                error => trigger.ReplyError("Account {0} not banned : {1}", accountName, error.Message));
        }
    }

    public class UnBanCommand : CommandBase
    {
        public UnBanCommand()
        {
            Aliases = new[] { "unban" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Unban an character";

            AddParameter<string>("player", "character", "Player name", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            var player = World.Instance.GetCharacter(trigger.Get<string>("player"));

            if (player == null)
                WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
                {
                    var character =
                        CharacterManager.Instance.GetCharacterByName(trigger.Get<string>("player"));

                    if (character == null)
                    {
                        trigger.ReplyError("Player {0} not found", trigger.Get<string>("player"));
                        return;
                    }

                    var message = new AccountRequestMessage { CharacterId = character.Id };
                    IPCAccessor.Instance.SendRequest<AccountAnswerMessage>(message,
                        reply => IPCAccessor.Instance.SendRequest(new UnBanAccountMessage(reply.Account.Login),
                            ok => trigger.Reply("Account {0} unbanned", reply.Account.Login),
                            error =>
                                trigger.ReplyError("Account {0} not unbanned : {1}", reply.Account.Login, error.Message)),
                        error =>
                            trigger.ReplyError("Player {0} not unbanned : {1}", character.Name, error.Message));
                });
            else
            {
                IPCAccessor.Instance.SendRequest(new UnBanAccountMessage(player.Account.Login),
                    ok => trigger.Reply("Account {0} unbanned", player.Account.Login),
                    error =>
                        trigger.ReplyError("Account {0} not unbanned : {1}", player.Account.Login, error.Message));

                if (!player.Account.IsJailed)
                    return;

                player.Account.IsJailed = false;
                player.Account.BanEndDate = null;

                player.Teleport(player.Breed.GetStartPosition());

                player.SendServerMessage("Vous avez été libéré de prison.", Color.Red);
            }
        }
    }

    public class UnBanAccountCommand : CommandBase
    {
        public UnBanAccountCommand()
        {
            Aliases = new[] { "unbanacc" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Unban an account";

            AddParameter<string>("character", "account", "Account login");
        }

        public override void Execute(TriggerBase trigger)
        {
            var accountName = trigger.Get<string>("account");

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            IPCAccessor.Instance.SendRequest(new UnBanAccountMessage(accountName),
                ok => trigger.Reply("Account {0} unbanned", accountName),
                error => trigger.ReplyError("Account {0} not unbanned : {1}", accountName, error.Message));
        }
    }
}