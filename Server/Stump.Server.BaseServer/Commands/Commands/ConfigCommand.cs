using System;
using System.IO;
using Stump.Server.BaseServer.Plugins;
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
            ParentCommandType = typeof(ConfigCommand);
            Aliases = new[] { "reload" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Reload the config file";
            AddParameter<string>("plugin", "p", "Plugin name whose config will be reloaded", isOptional: true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (!trigger.IsArgumentDefined("plugin"))
            {
                ServerBase.InstanceAsBase.Config.Reload();

                trigger.Reply("Config reloaded");
            }
            else
            {
                var pluginName = trigger.Get<string>("plugin");
                var plugin = PluginManager.Instance.GetPlugin(pluginName, true);
                if (plugin == null)
                    trigger.ReplyError($"Plugin '{pluginName}' not found, check plugins list");
                else if (plugin.Plugin.Config == null)
                    trigger.ReplyError($"Plugin '{pluginName}' has no config");
                else
                {
                    plugin.Plugin.Config.Reload();
                    trigger.Reply($"Config from plugin '{pluginName}' reloaded");
                }
            }   

        }
    }

    public class ConfigSaveCommand : SubCommand
    {
        public ConfigSaveCommand()
        {
            ParentCommandType = typeof(ConfigCommand);
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