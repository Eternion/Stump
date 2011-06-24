
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static List<string> PluginsPath = new List<string>
            {
                "./plugins/"
            };

        public delegate void PluginContextHandler(PluginContext pluginContext);

        public event PluginContextHandler PluginAdded;

        private void InvokePluginAdded(PluginContext pluginContext)
        {
            PluginContextHandler handler = PluginAdded;
            if (handler != null) handler(pluginContext);
        }

        public event PluginContextHandler PluginRemoved;

        private void InvokePluginRemoved(PluginContext pluginContext)
        {
            PluginContextHandler handler = PluginRemoved;
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
                if(!File.Exists(path))
                {
                    logger.Error("Cannot load unexistant plugin path {0}", path);
                    continue;
                }

                if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
                {
                    foreach (var file in Directory.EnumerateFiles(path))
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

            Assembly pluginAssembly = Assembly.LoadFrom(libPath);
            var pluginContext = new PluginContext(libPath, pluginAssembly);
            bool initialized = false;

            // search the entry point (the class that implements IPlugin)
            foreach (Type pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.IsPublic && !pluginType.IsAbstract)
                {
                    if (pluginType.HasInterface(typeof(IPlugin)))
                    {
                        if (initialized)
                            throw new PluginLoadException("Found 2 classes that implements IPlugin. A plugin can contains only one");

                        pluginContext.Initialize(pluginType);
                        initialized = true;

                        RegisterPlugin(pluginContext);
                    }
                }
            }

            return pluginContext;
        }

        public void UnLoadPlugin(string name)
        {
            var plugin = from entry in PluginContexts
                         where entry.Plugin.Name.Equals(name)
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