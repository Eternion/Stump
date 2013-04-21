using System;

namespace Sniffer.Modules
{
    public abstract class BaseModule
    {
        public virtual string GetName()
        {
            return "#NO_NAME#";
        }

        public virtual string GetAuthor()
        {
            return "#NO_AUTHOR#";
        }

        public virtual string GetVersion()
        {
            return "#UNKNOW_VERSION#";
        }

        public virtual void Initialize()
        {
            
        }

        public virtual void Run()
        {
            
        }

        public virtual void Stop()
        {

        }
    }
}
