
using System.IO;
using System.Reflection;
using Stump.Core.Xml;

namespace Stump.Server.BaseServer.Plugins
{
    public abstract class PluginBase : IPlugin
    {
        public abstract bool UseConfig
        {
            get;
        }

        public abstract string ConfigFileName
        {
            get;
        }

        public XmlConfigReader ConfigReader
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

        public abstract string Version
        {
            get;
        }

        public abstract void Initialize();
        public abstract void Shutdown();
        public abstract void Dispose();

        #endregion

        public virtual void LoadConfig(PluginContext context)
        {
            if (UseConfig)
            {
                ConfigReader =
                    new XmlConfigReader(Path.Combine(Path.GetDirectoryName(context.AssemblyPath),
                                                   !string.IsNullOrEmpty(ConfigFileName)
                                                       ? ConfigFileName
                                                       : Name + ".xml"));
                ConfigReader.DefinesVariables(GetType().Assembly);
            }
        }
    }
}