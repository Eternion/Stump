
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.AuthServer.Database
{
    public abstract class AuthBaseRecord<T> : ActiveRecordBase<T>
    {
        public RecordState State
        {
            get;
            set;
        }
    }
}
