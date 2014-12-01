using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Mounts
{
    public class MountManager : DataManager<MountManager>
    {
        private UniqueIdProvider m_idProvider;
        private Dictionary<int, MountTemplate> m_mountTemplates;
        private Dictionary<int, MountRecord> m_mountRecords;

        private readonly object m_lock = new object();

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_mountTemplates = Database.Query<MountTemplate>(MountTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_mountRecords = Database.Query<MountRecord>(MountRecordRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_idProvider = m_mountRecords.Any() ? new UniqueIdProvider(m_mountRecords.Select(x => x.Value.Id).Max()) : new UniqueIdProvider(1);
        }

        public MountTemplate[] GetTemplates()
        {
            return m_mountTemplates.Values.ToArray();
        }

        public MountTemplate GetTemplate(int id)
        {
            MountTemplate result;
            return !m_mountTemplates.TryGetValue(id, out result) ? null : result;
        }

        public Mount CreateMount(Character character, bool sex, int modelid)
        {
            var mount = new Mount(character, m_idProvider.Pop(), sex, modelid);

            AddMount(mount);

            return mount;
        }

        public void AddMount(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Insert(mount.Record));

            lock (m_lock)
            {
                mount.Record.IsNew = false;
                m_mountRecords.Add(mount.Id, mount.Record);
            }
        }

        public bool DeleteMount(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Delete(mount.Record));

            lock (m_lock)
            {
                m_mountRecords.Remove(mount.Id);
                return true;
            }
        }

        public MountRecord TryGetMount(int characterId)
        {
            lock (m_lock)
            {
                return m_mountRecords.FirstOrDefault(x => x.Value.OwnerId == characterId).Value;
            }
        }
    }
}
