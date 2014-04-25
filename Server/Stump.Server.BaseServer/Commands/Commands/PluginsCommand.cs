using System;
using System.IO;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class PluginsCommand : SubCommandContainer
    {
        public PluginsCommand()
        {
            Aliases = new[] {"plugins"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Provide commands to manage plugins";
        }
    }

    public class PluginLoadCommand : SubCommand
    {
        public PluginLoadCommand()
        {
            ParentCommandType = typeof (PluginsCommand);
            Aliases = new[] {"load"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Load a plugin";
            AddParameter<string>("name", "n", "Plugin name or path (in plugins directory)");
        }

        public override void Execute(TriggerBase trigger)
        {
            var path = trigger.Get<string>("name");

            if (Directory.Exists(path))
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.dll", path.EndsWith("*")
                                                                                    ? SearchOption.AllDirectories
                                                                                    : SearchOption.TopDirectoryOnly))
                {
                    if (PluginManager.Instance.GetPlugins().Any(entry => Path.GetFullPath(entry.AssemblyPath) == Path.GetFullPath(file)))
                        continue;

                    PluginContext plugin = PluginManager.Instance.LoadPlugin(file);

                    trigger.Reply("Plugin {0} loaded", plugin.Plugin.Name);
                }
            }
            else if (File.Exists(path))
            {
                PluginContext plugin = PluginManager.Instance.LoadPlugin(path);

                trigger.Reply("Plugin {0} loaded", plugin.Plugin.Name);
            }
            else
            {
                foreach (var pluginPath in PluginManager.PluginsPath)
                {
                    if (!Directory.Exists(pluginPath))
                        continue;

                    foreach (string plugin in Directory.EnumerateFiles(pluginPath, "*.dll", SearchOption.AllDirectories))
                    {
                        if (PluginManager.Instance.GetPlugins().Any(entry => Path.GetFullPath(entry.AssemblyPath) == Path.GetFullPath(plugin)))
                            continue;

                        if (Path.GetFileNameWithoutExtension(plugin).Equals(Path.GetFileNameWithoutExtension(path), StringComparison.InvariantCultureIgnoreCase)
                            || Path.GetFileName(plugin).Equals(Path.GetFileName(plugin), StringComparison.InvariantCultureIgnoreCase))
                        {
                            PluginContext pluginContext = PluginManager.Instance.LoadPlugin(plugin);

                            trigger.Reply("Plugin {0} loaded", pluginContext.Plugin.Name);
                        }
                    }
                }
            }
        }
    }

    public class PluginReloadCommand : SubCommand
    {
        public PluginReloadCommand()
        {
            ParentCommandType = typeof (PluginsCommand);
            Aliases = new[] { "reload" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Reload and reset a plugin";
            AddParameter<string>("name", "n", "Plugin name");
            AddParameter("ignoreCase", "case", "Ignore case for name comparaison", true);
        }

        public override void Execute(TriggerBase trigger)
        {
            PluginContext plugin = PluginManager.Instance.GetPlugin(trigger.Get<string>("name"), trigger.Get<bool>("case"));

            if (plugin == null)
            {
                trigger.ReplyError("Plugin not found");
                return;
            }

            PluginManager.Instance.UnLoadPlugin(plugin);
            PluginManager.Instance.LoadPlugin(plugin.AssemblyPath);

            trigger.Reply("Plugin {0} reloaded", plugin.Plugin.Name);
        }
    }

    public class PluginUnloadCommand : SubCommand
    {
        public PluginUnloadCommand()
        {
            ParentCommandType = typeof (PluginsCommand);
            Aliases = new[] {"unload"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Unload a plugin";
            AddParameter<string>("name", "n", "Plugin name");
            AddParameter("ignoreCase", "case", "Ignore case for name comparaison", true);
        }

        public override void Execute(TriggerBase trigger)
        {
            PluginContext plugin = PluginManager.Instance.GetPlugin(trigger.Get<string>("name"), trigger.Get<bool>("case"));

            if (plugin == null)
            {
                trigger.ReplyError("Plugin not found");
                return;
            }

            PluginManager.Instance.UnLoadPlugin(plugin);

            trigger.Reply("Plugin {0} unloaded", plugin.Plugin.Name);
        }
    }

    public class PluginInfoCommand : SubCommand
    {
        public PluginInfoCommand()
        {
            ParentCommandType = typeof(PluginsCommand);
            Aliases = new[] { "info" };
            RequiredRole = RoleEnum.Administrator;
            Description = "Get info. about a plugin";
            AddParameter<string>("name", "n", "Plugin name");
            AddParameter("ignoreCase", "case", "Ignore case for name comparaison", true);
        }

        public override void Execute(TriggerBase trigger)
        {
            PluginContext plugin = PluginManager.Instance.GetPlugin(trigger.Get<string>("name"), trigger.Get<bool>("case"));

            if (plugin == null)
            {
                trigger.ReplyError("Plugin not found");
                return;
            }

            trigger.Reply("Plugin {0} v.{1} from {2} : {3}", plugin.Plugin.Name, plugin.Plugin.Version, plugin.Plugin.Author, plugin.Plugin.Description);
        }
    }

    public class PluginListCommand : SubCommand
    {
        public PluginListCommand()
        {
            ParentCommandType = typeof(PluginsCommand);
            Aliases = new[] { "list" };
            RequiredRole = RoleEnum.Administrator;
            Description = "List all loaded plugins";
        }

        public override void Execute(TriggerBase trigger)
        {
            var plugins = PluginManager.Instance.GetPlugins();

            if (plugins.Length == 0)
            {
                trigger.Reply("No plugin loaded");
            }

            foreach (var plugin in plugins)
            {
                trigger.Reply("- {0} : {1}", plugin.Plugin.Name, plugin.Plugin.Description);
            }
        }
    }
}