using System;
using ArkalysPlugin.Commands;
using Stump.Core.Attributes;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Plugins;
using Stump.Server.WorldServer;

namespace ArkalysPlugin
{
    public class Plugin : PluginBase
    {
        private bool m_configAutoReload;

        public Plugin(PluginContext context)
            : base(context)
        {
            CurrentPlugin = this;
        }

        public override string Name
        {
            get { return "Arkalys Plugin"; }
        }

        public override string Description
        {
            get { return "This plugin manage the gameplay of arkalys server"; }
        }

        public override string Author
        {
            get { return "bouh2"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public override void Initialize()
        {
            base.Initialize();

            if (!m_configAutoReload)
            {
                m_configAutoReload = true;
                WorldServer.Instance.StartConfigReloadOnChange(Config);
            }

            Initialized = true;
        }

        public override void Shutdown()
        {
            base.Shutdown();

            IsleTrigger.TearDown();

            if (Config != null)
                WorldServer.Instance.StopConfigReloadOnChange(Config);

            Initialized = false;
        }

        public override void Dispose()
        {

        }

        public override bool UseConfig
        {
            get { return true; }
        }

        public override string ConfigFileName
        {
            get { return "arkalys_plugin.xml"; }
        }

        public static Plugin CurrentPlugin
        {
            get;
            private set;
        }

        public bool Initialized
        {
            get;
            private set;
        }
    }
}