namespace Stump.Server.BaseServer.Plugins
{
    public interface IPlugin
    {
        string Name
        {
            get;
        }

        string Description
        {
            get;
        }

        string Author
        {
            get;
        }

        string Version
        {
            get;
        }

        void LoadConfig(PluginContext context);

        void Initialize();

        void Dispose();
    }
}