
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;

namespace Stump.Server.WorldServer.Database
{
    public abstract class WorldBaseRecord<T> : ActiveRecordBase<T>
    {
        public void SaveLater()
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() => Save());
        }

        public void UpdateLater()
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() => Update());
        }

        public void CreateLater()
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() => Create());
        }

        public void DeleteLater()
        {
            WorldServer.Instance.IOTaskPool.AddMessage(() => Delete());
        }
    }
}
