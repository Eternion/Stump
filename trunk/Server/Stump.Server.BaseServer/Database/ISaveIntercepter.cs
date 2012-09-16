using System.Data.Objects;

namespace Stump.Server.BaseServer.Database
{
    public interface ISaveIntercepter
    {
        void BeforeSave(ObjectStateEntry currentEntry);
    }
}