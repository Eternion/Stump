
using System;
using System.IO;
using System.Reflection;
using Stump.Core.Xml;
using Stump.Core.Xml.Config;

namespace Stump.Server.BaseServer.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        public abstract bool UseConfig
        {
            get;
        }

        public abstract string ConfigFileName
        {
            get;
        }

        public XmlConfig Config
        {
            get;
            protected set;
        }

        #region IPlugin Members

        public abstract string Name
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public abstract string Author
        {
            get;
        }

        public abstract string Version
        {
            get;
        }

        public abstract void Initialize();
        public abstract void Shutdown();
        public abstract void Dispose();

        #endregion

        public virtual void LoadConfig(PluginContext context)
        {
            if (UseConfig)
            {
                Config =
                    new XmlConfig(Path.Combine(Path.GetDirectoryName(context.AssemblyPath),
                                                   !string.IsNullOrEmpty(ConfigFileName)
                                                       ? ConfigFileName
                                                       : Name + ".xml"));
                Config.AddAssembly(GetType().Assembly);
                Config.Load();
            }
        }
    }
}