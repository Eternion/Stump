using Stump.Server.BaseServer.Plugins;
using System;

namespace Stump.Plugins.WebServerPlugin
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
            get { return "Stump WebServer"; }
        }

        public override string Description
        {
            get { return "Mini WebServer"; }
        }

        public override string Author
        {
            get { return "Orochi"; }
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
