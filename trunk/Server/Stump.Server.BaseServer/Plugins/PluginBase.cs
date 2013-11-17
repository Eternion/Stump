
using System;
using System.IO;
using Stump.Core.Xml.Config;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Initialization;

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

        public virtual bool AllowConfigUpdate
        {
            get { return UseConfig; }
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

        public abstract Version Version
        {
            get;
        }

        public virtual void Initialize()
        {
            InitializationManager.Instance.AddAssembly(Context.PluginAssembly);

            if (ServerBase.InstanceAsBase.IsInitialized)
            {
                // just to ensure the context
                ServerBase.InstanceAsBase.IOTaskPool.AddMessage(InitializationManager.Instance.InitializeAll);
            }

            CommandManager.Instance.RegisterAll(Context.PluginAssembly);
        }

        public virtual void Shutdown()
        {
            CommandManager.Instance.UnRegisterAll(Context.PluginAssembly);
        }

        public abstract void Dispose();

        #endregion

        public virtual void LoadConfig()
        {
            if (!UseConfig)
                return;

            var configPath = GetConfigPath();
            Config = new XmlConfig(configPath);
            Config.AddAssembly(GetType().Assembly);

            if (File.Exists(configPath))
                Config.Load();
            else
                Config.Create();
        }

        public virtual string GetConfigPath()
        {
            return Path.Combine(GetPluginDirectory(), !string.IsNullOrEmpty(ConfigFileName) ? ConfigFileName : Name + ".xml");
        }

        public string GetPluginDirectory()
        {
            return Path.GetDirectoryName(Context.AssemblyPath);
        }
    }
}