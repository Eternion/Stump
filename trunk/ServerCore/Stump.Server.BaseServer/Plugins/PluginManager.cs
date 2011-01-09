// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NLog;
using Stump.BaseCore.Framework.Attributes;

namespace Stump.Server.BaseServer.Plugins
{
    public static class PluginManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static IList<string> PluginsPath = new List<string>
            {
                "./plugins/"
            };

        public delegate void PluginContextHandler(PluginContext pluginContext);

        public static event PluginContextHandler PluginAdded;

        private static void InvokePluginAdded(PluginContext pluginContext)
        {
            PluginContextHandler handler = PluginAdded;
            if (handler != null) handler(pluginContext);
        }

        public static event PluginContextHandler PluginRemoved;

        private static void InvokePluginRemoved(PluginContext pluginContext)
        {
            PluginContextHandler handler = PluginRemoved;
            if (handler != null) handler(pluginContext);
        }

        public static readonly IList<PluginContext> PluginContexts = new List<PluginContext>();

        public static void LoadAllPlugins()
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

        public static void LoadPlugin(string libPath)
        {
            if (!File.Exists(libPath))
                throw new FileNotFoundException("File doesn't exist", libPath);

            Assembly pluginAssembly = Assembly.LoadFrom(libPath);

            foreach (Type pluginType in pluginAssembly.GetTypes())
            {
                if (pluginType.IsPublic && !pluginType.IsAbstract)
                {
                    Type pluginInterface = pluginType.GetInterface(typeof (IPlugin).Name);
                    if (pluginInterface != null)
                    {
                        var plugin = new PluginContext(libPath, pluginAssembly);

                        plugin.InitPlugin();
                    }
                }
            }
        }

        public static string GetDefaultDescription(this IPlugin plugin)
        {
            return string.Format("{0} v{1} by {2}", plugin.Name, plugin.GetType().Assembly.GetName().Version, plugin.Author);
        }

        internal static void RegisterPlugin(PluginContext pluginContext)
        {
            PluginContexts.Add(pluginContext);

            InvokePluginAdded(pluginContext);
        }

        internal static void UnRegisterPlugin(PluginContext pluginContext)
        {
            PluginContexts.Remove(pluginContext);

            InvokePluginRemoved(pluginContext);
        }
    }
}