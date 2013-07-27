using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.BaseServer.Plugins;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.AI.Fights.Brain;

namespace GiantKralovePlugin
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
            get { return "GiantKralove Plugin"; }
        }

        public override string Description
        {
            get { return "This plugin manage the ai and the map of giant kralove."; }
        }

        public override string Author
        {
            get { return "Novior"; }
        }

        public override Version Version
        {
            get { return new Version(0, 0); }
        }

        public override void Initialize()
        {
            BrainManager.Instance.RegisterBrain(typeof (GiantKraloveBrain));
            Initialized = true;
        }

        public override bool UseConfig
        {
            get { return false; }
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

        public override void Dispose()
        {

        }
    }
}
