using System;
using System.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class BanCommand : CommandBase
    {
        public static TimeSpan LifeBanValue
        {
            get { return new DateTime(2038, 1, 19) - DateTime.Now; } // max unix timestamp
        }

        public BanCommand()
        {
            Aliases = new[] { "ban" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Ban a player";

            AddParameter("target", "t", "Player to ban", converter: ParametersConverter.CharacterConverter);
            AddParameter<int>("time", "time", "Ban duration (in minutes)", isOptional: true);
            AddParameter("reason", "r", "Reason of ban", "No reason");
            AddParameter<bool>("life", "l", "Specify a life ban", isOptional: true);
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
            
            if (!IpcAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            if (trigger.IsArgumentDefined("time"))
            {
                var time = trigger.Get<int>("time");
                var source = trigger.GetSource() as WorldClient;

                if (source != null)
                    AccountManager.Instance.BanLater(target.Account, source.Account, TimeSpan.FromMinutes(time), reason);
                else
                    AccountManager.Instance.BanLater(target.Account, TimeSpan.FromMinutes(time), reason);
            }
            else if (trigger.IsArgumentDefined("life"))
            {
                var source = trigger.GetSource() as WorldClient;

                if (source != null)
                    AccountManager.Instance.BanLater(target.Account, source.Account, LifeBanValue, reason);
                else
                    AccountManager.Instance.BanLater(target.Account, LifeBanValue, reason);
            }

            target.Client.Disconnect();
            trigger.Reply("Account {0} banned", target.Account.Nickname);
        }
    }

    public class BanIpCommand : CommandBase
    {
        public BanIpCommand()
        {
            Aliases = new[] { "banip" };
            RequiredRole = RoleEnum.Moderator;
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

            if (!IpcAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            var source = trigger.GetSource() as WorldClient;
            var sourceId = source != null ? source.Account.Id : 0;

            var time = trigger.IsArgumentDefined("time") ? TimeSpan.FromMinutes(trigger.Get<int>("time")) : BanCommand.LifeBanValue;

            if (trigger.IsArgumentDefined("life"))
                time = TimeSpan.MaxValue;

            WorldServer.Instance.IOTaskPool.AddMessage(() => IpcAccessor.Instance.ProxyObject.BanIp(ip, sourceId, time, reason));

            trigger.Reply("Ip {0} banned", ip);
        }
    }

    public class BanAccountCommand : CommandBase
    {
        public BanAccountCommand()
        {
            Aliases = new[] {"banacc"};
            RequiredRole = RoleEnum.Moderator;
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

            if (!IpcAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            var source = trigger.GetSource() as WorldClient;
            var sourceId = source != null ? source.Account.Id : 0;

            var time = trigger.IsArgumentDefined("time")
                           ? TimeSpan.FromMinutes(trigger.Get<int>("time"))
                           : TimeSpan.MaxValue;

            if (trigger.IsArgumentDefined("life"))
                time = BanCommand.LifeBanValue;

            WorldServer.Instance.IOTaskPool.AddMessage(
                () => IpcAccessor.Instance.ProxyObject.BlamAccount(accountName, sourceId, time, reason));

            trigger.Reply("Account {0} banned", accountName);
        }
    }


    public class UnBanAccountCommand : CommandBase
    {
        public UnBanAccountCommand()
        {
            Aliases = new[] { "unban" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Unban an account";

            AddParameter<string>("account", "account", "Account login");
        }

        public override void Execute(TriggerBase trigger)
        {
            var accountName = trigger.Get<string>("account");
           
            if (!IpcAccessor.Instance.IsConnected)
            {
                trigger.ReplyError("IPC service not operational !");
                return;
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() => IpcAccessor.Instance.ProxyObject.UnBlamAccount(accountName));

            trigger.Reply("Account {0} unbanned", accountName);
        }
    }
}