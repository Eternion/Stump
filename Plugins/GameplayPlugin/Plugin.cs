using System;
using Stump.Server.BaseServer.Plugins;

namespace GameplayPlugin
{
    public class Plugin : PluginBase
    {
        public Plugin(PluginContext context)
            : base(context)
        {
            CurrentPlugin = this;
        }

        public override string Name
        {
            get { return "Gameplay Plugin"; }
        }

        public override string Description
        {
            get { return "This plugin manage the gameplay of server"; }
        }

        public override string Author
        {
            get { return "Azote"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public override void Initialize()
        {
            base.Initialize();
            Initialized = true;
        }

        public override void Shutdown()
        {
            base.Shutdown();

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
            get { return "gameplay_plugin.xml"; }
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