using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class BanCommand : CommandBase
    {
        public BanCommand()
        {
            Aliases = new[] { "ban" };
            RequiredRole = RoleEnum.GameMaster_Padawan;
            Description = "Ban a player";

            AddParameter("target", "t", "Player to ban", converter: ParametersConverter.CharacterConverter);
            AddParameter<int>("time", "time", "Ban duration (in minutes)", isOptional: true);
            AddParameter("reason", "r", "Reason of ban", "No reason");
            AddParameter<bool>("life", "l", "Specify a life ban", isOptional: true);
            AddParameter<bool>("ip", "ip", "Also ban the ip", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = trigger.Get<Character>("target");
            var reason = trigger.Get<string>("reason");

            if (target == null)
            {
                trigger.ReplyError("Define a target !");
                return;
            }

            if (!IPCAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            var message = new BanAccountMessage()
            {
                AccountId = target.Account.Id,
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

            if ((trigger.Get<int>("time") > (60 * 60 * 24) || message.BanEndDate == null) && source.Account.Role == RoleEnum.GameMaster_Padawan)
                message.BanEndDate = DateTime.Now + TimeSpan.FromMinutes((60 * 60 * 24));

            target.Client.Disconnect();

            IPCAccessor.Instance.SendRequest(message, 
                ok => trigger.Reply("Account {0} banned", target.Account.Login),
                error => trigger.ReplyError("Account {0} not banned : {1}", target.Account.Login, error.Message));


            if (!trigger.IsArgumentDefined("ip"))
                return;

            var banIPMessage = new BanIPMessage()
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

            var message = new BanIPMessage()
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

    public class BanAccountCommand : CommandBase
    {
        public BanAccountCommand()
        {
            Aliases = new[] {"banacc"};
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

            var message = new BanAccountMessage()
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


    public class UnBanAccountCommand : CommandBase
    {
        public UnBanAccountCommand()
        {
            Aliases = new[] { "unban" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Unban an account";

            AddParameter<string>("account", "account", "Account login");
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