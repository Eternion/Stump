using System.Collections.Generic;
using System.Linq;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Mounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Paddocks;
using Stump.Server.WorldServer.Handlers.Mounts;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Mounts
{
    public class MountManager : DataManager<MountManager>
    {
        private Dictionary<int, MountTemplate> m_mountTemplates;

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_mountTemplates = Database.Query<MountTemplate>(MountTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
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
            var mount = new Mount(sex, modelid);

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                AddMount(mount);
                LinkMountToCharacter(character, mount);

                mount.Owner = character;
                character.Mount = mount;

                MountHandler.SendMountSetMessage(character.Client, mount.GetMountClientData());
            });  

            return mount;
        }

        public void LinkMountToCharacter(Character character, Mount mount)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() => {
                var record = new MountCharacter
                {
                    CharacterId = character.Id,
                    MountId = mount.Id
                };

                Database.Insert(record);
            });
        }

        public void UnlinkMountFromCharacter(Character character)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() => {
                var record = Database.Query<MountCharacter>(string.Format(MountCharacterRelator.FetchByCharacterId, character.Id)).FirstOrDefault();
                Database.Delete(record);     
            });
        }

        public void LinkMountToPaddock(Paddock paddock, Mount mount, bool stabled)
        {
            var record = new MountPaddock
            {
                PaddockId = paddock.Id,
                MountId = mount.Id,
                CharacterId = mount.OwnerId,
                Stabled = stabled
            };

            WorldServer.Instance.IOTaskPool.ExecuteInContext(
                () => Database.Insert(record));
        }

        public void UnlinkMountFromPaddock(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
            {
                var record = Database.Query<MountPaddock>(string.Format(MountPaddockRelator.FetchByMountId, mount.Id)).FirstOrDefault();
                Database.Delete(record);
            });
        }

        public void AddMount(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.ExecuteInContext
                (() => Database.Insert(mount.Record));

            mount.Record.IsNew = false;
        }

        public void DeleteMount(Mount mount)
        {
            WorldServer.Instance.IOTaskPool.AddMessage(
                () => Database.Delete(mount.Record));
        }

        public MountRecord TryGetMountByCharacterId(int characterId)
        {
            var record = Database.Query<MountCharacter>(string.Format(MountCharacterRelator.FetchByCharacterId, characterId)).FirstOrDefault();
            return record == null ? null : TryGetMount(record.MountId);
        }

        public MountRecord TryGetMount(int mountId)
        {
            return Database.Query<MountRecord>(string.Format(MountRecordRelator.FindById, mountId)).FirstOrDefault();
        }

        public List<MountRecord> TryGetMountsByPaddockId(int paddockId, bool stabled)
        {
            return Database.Fetch<MountRecord>(string.Format(MountRecordRelator.FetchByPaddockId, paddockId));
        }

        public Mount GetMountById(int mountId)
        {
            var record = TryGetMount(mountId);
            return record == null ? null : new Mount(record);
        }
    }
}
