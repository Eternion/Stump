using System;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Plugins.DefaultPlugin
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
            get { return "Default Plugin"; }
        }

        public override string Description
        {
            get { return "This plugin contains additions and fixes to Stump (gameplay fixes)"; }
        }

        public override string Author
        {
            get { return "bouh2"; }
        }

        public override Version Version
        {
            get
            {
                return new Version(1, 0);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Shutdown()
        {

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
            get { return "default_plugin_config.xml"; }
        }

        public static Plugin CurrentPlugin
        {
            get;
            private set;
        }
    }
}