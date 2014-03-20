
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Plugins
{
    public sealed class PluginManager : Singleton<PluginManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable(true)]
        public static List<string> PluginsPath = new List<string>{"./plugins/"};

        public delegate void PluginContextHandler(PluginContext pluginContext);

        public event PluginContextHandler PluginAdded;

        private void InvokePluginAdded(PluginContext pluginContext)
        {
            var handler = PluginAdded;
            if (handler != null) handler(pluginContext);
        }

        public event PluginContextHandler PluginRemoved;

        private void InvokePluginRemoved(PluginContext pluginContext)
        {
            var handler = PluginRemoved;
            if (handler != null) handler(pluginContext);
        }

        internal readonly IList<PluginContext> PluginContexts = new List<PluginContext>();

        private PluginManager()
        {

        }

        public void LoadAllPlugins()
        {
            foreach (var path in PluginsPath)
            {
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    logger.Error("Cannot load unexistant plugin path {0}", path);
                    return;
                }

                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                {
                    foreach (var file in Directory.EnumerateFiles(path, "*.dll"))
                    {
                        LoadPlugin(file);
                    }
                }
                else
                    LoadPlugin(path);
            }
        }

        public PluginContext LoadPlugin(string libPath)
        {
            if (!File.Exists(libPath))
                throw new FileNotFoundException("File doesn't exist", libPath);

            if (PluginContexts.Any(entry => Path.GetFullPath(entry.AssemblyPath) == Path.GetFullPath(libPath)))
                throw new Exception("Plugin already loaded");

            var asmData = File.ReadAllBytes(libPath);

            var pluginAssembly = Assembly.Load(asmData);
            var pluginContext = new PluginContext(libPath, pluginAssembly);
            var initialized = false;

            // search the entry point (the class that implements IPlugin)
            foreach (var pluginType in pluginAssembly.GetTypes().Where(pluginType => pluginType.IsPublic && !pluginType.IsAbstract).Where(pluginType => pluginType.HasInterface(typeof (IPlugin))))
            {
                if (initialized)
                    throw new PluginLoadException("Found 2 classes that implements IPlugin. A plugin can contains only one");

                pluginContext.Initialize(pluginType);
                initialized = true;

                RegisterPlugin(pluginContext);
            }

            return pluginContext;
        }

        public void UnLoadPlugin(string name, bool ignoreCase = false)
        {
            var plugin = from entry in PluginContexts
                         where entry.Plugin.Name.Equals(name, ignoreCase ?
                            StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                         select entry;

            foreach (var pluginContext in plugin)
            {
                UnLoadPlugin(pluginContext);
            }
        }

        public void UnLoadPlugin(PluginContext context)
        {
            context.Plugin.Shutdown();
            context.Plugin.Dispose();

            UnRegisterPlugin(context);
        }

        public PluginContext GetPlugin(string name, bool ignoreCase = false)
        {
            var plugins = from entry in PluginContexts
                         where entry.Plugin.Name.Equals(name, ignoreCase ?
                            StringComparison.InvariantCultureIgnoreCase : StringComparison.InvariantCulture)
                         select entry;

            return plugins.FirstOrDefault();
        }

        public PluginContext[] GetPlugins()
        {
            return PluginContexts.ToArray();
        }

        internal void RegisterPlugin(PluginContext pluginContext)
        {
            PluginContexts.Add(pluginContext);

            InvokePluginAdded(pluginContext);
        }

        internal void UnRegisterPlugin(PluginContext pluginContext)
        {
            PluginContexts.Remove(pluginContext);

            InvokePluginRemoved(pluginContext);
        }
    }

    public class PluginLoadException : Exception
    {
        public PluginLoadException(string exception)
            : base(exception)
        {

        }
    }
}