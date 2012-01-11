
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.AuthServer.Database
{
    public abstract class AuthBaseRecord<T> : ActiveRecordBase<T>
    {
        public void SaveLater()
        {
            AuthServer.Instance.IOTaskPool.AddMessage(() => Save());
        }

        public void UpdateLater()
        {
            AuthServer.Instance.IOTaskPool.AddMessage(() => Update());
        }

        public void CreateLater()
        {
            AuthServer.Instance.IOTaskPool.AddMessage(() => Create());
        }

        public void DeleteLater()
        {
            AuthServer.Instance.IOTaskPool.AddMessage(() => Delete());
        }
    }
}
