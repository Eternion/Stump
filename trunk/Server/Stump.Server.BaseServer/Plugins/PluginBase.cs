
using System;
using System.IO;
using System.Reflection;
using Stump.Core.Xml;
using Stump.Core.Xml.Config;

namespace Stump.Server.BaseServer.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        public virtual bool UseConfig
        {
            get { return false; }
        }

        public virtual string ConfigFileName
        {
            get { return null; }
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
            if (!UseConfig)
                return;

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