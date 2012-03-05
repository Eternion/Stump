using System;
using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Plugins.DebugPlugin
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
            get { return "Debug Plugin"; }
        }

        public override string Description
        {
            get { return "Provide methods and commands to debug"; }
        }

        public override string Author
        {
            get { return "bouh2"; }
        }

        public override string Version
        {
            get { return "1.0.0.0"; }
        }

        public override void Initialize()
        {
        }

        public override void Shutdown()
        {
        }

        public override void Dispose()
        {

        }

        public static Plugin CurrentPlugin
        {
            get;
            private set;
        }
    }
}