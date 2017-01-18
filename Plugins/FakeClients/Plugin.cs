using System;
using FakeClients.Handlers;
using Stump.Server.BaseServer.Plugins;

namespace FakeClients
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
            get { return "Fake clients"; }
        }

        public override string Description
        {
            get { return "Simulate client connections"; }
        }

        public override string Author
        {
            get { return "Azote"; }
        }

        public override bool AllowConfigUpdate
        {
            get { return true; }
        }

        public override bool UseConfig
        {
            get { return true; }
        }

        public override string ConfigFileName
        {
            get { return "fake_client_config.xml"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0); }
        }

        public override void Initialize()
        {
            base.Initialize();
            Handler = new FakeClientPacketHandler();
            Handler.RegisterAll(Context.PluginAssembly);
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

        public FakeClientPacketHandler Handler
        {
            get;
            private set;
        }
    }
}
