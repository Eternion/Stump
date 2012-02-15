using Stump.Server.BaseServer.Commands;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Plugins.DefaultPlugin
{
    public class Plugin : PluginBase
    {
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
    }
}