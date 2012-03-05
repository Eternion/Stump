
using System;
using System.IO;
using System.Reflection;
using Stump.Core.Xml;
using Stump.Core.Xml.Config;

namespace Stump.Server.BaseServer.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        protected PluginBase(PluginContext context)
        {
            Context = context;
        }

        public PluginContext Context
        {
            get;
            protected set;
        }

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

        public virtual void LoadConfig()
        {
            if (!UseConfig)
                return;

            var configPath = Path.Combine(Path.GetDirectoryName(Context.AssemblyPath), !string.IsNullOrEmpty(ConfigFileName) ? ConfigFileName : Name + ".xml");
            Config = new XmlConfig(configPath);
            Config.AddAssembly(GetType().Assembly);

            if (File.Exists(configPath))
                Config.Load();
            else
                Config.Create();
        }
    }
}