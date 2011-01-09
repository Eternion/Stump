using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Stump.BaseCore.Framework.Xml;

namespace Stump.Server.BaseServer.Plugins
{
    public class PluginContext
    {
        public PluginContext(string assemblyPath, Assembly pluginAssembly)
        {
            AssemblyPath = assemblyPath;
            PluginAssembly = pluginAssembly;
        }

        public string AssemblyPath
        {
            get;
            private set;
        }

        public Assembly PluginAssembly
        {
            get;
            private set;
        }

        public IPlugin Plugin
        {
            get;
            private set;
        }

        public void InitPlugin()
        {
            if (Plugin == null)
            {
                foreach (var type in PluginAssembly.GetTypes())
                {
                    var interfaces = type.GetInterfaces();
                    if (interfaces.Contains(typeof(IPlugin)))
                    {
                        Plugin = (IPlugin)Activator.CreateInstance(type);

                        if (Plugin != null)
                        {
                            Plugin.LoadConfig(this);
                            Plugin.Initialize();

                            PluginManager.RegisterPlugin(this);
                        }
                        break;
                    }
                }
            }
        }

        public override string ToString()
        {
            if (Plugin == null)
            {
                return PluginAssembly.FullName;
            }
            return Plugin.Name + " : " + Plugin.Description;
        }
    }
}