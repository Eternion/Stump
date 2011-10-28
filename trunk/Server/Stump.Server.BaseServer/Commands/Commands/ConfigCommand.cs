using System;
using System.IO;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class ConfigCommand : SubCommandContainer
    {
        public ConfigCommand()
        {
            Aliases = new [] { "config" };
            Description = "Provide commands to manage the config file";
            RequiredRole = RoleEnum.Administrator;
        }
    }

    public class ConfigReloadCommand : SubCommand
    {
        public ConfigReloadCommand()
        {
            ParentCommand = typeof(ConfigCommand);
            Aliases = new[] { "reload" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Reload the config file";
        }

        public override void Execute(TriggerBase trigger)
        {
            ServerBase.InstanceAsBase.Config.Reload();

            trigger.Reply("Config reloaded");
        }
    }

    public class ConfigSaveCommand : SubCommand
    {
        public ConfigSaveCommand()
        {
            ParentCommand = typeof(ConfigCommand);
            Aliases = new[] { "save" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Save the config file";
        }

        public override void Execute(TriggerBase trigger)
        {
            ServerBase.InstanceAsBase.Config.Save();

            trigger.Reply("Config saved");
        }
    }
}