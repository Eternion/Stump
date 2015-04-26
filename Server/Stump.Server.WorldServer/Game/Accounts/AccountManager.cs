using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Accounts
{
    public class AccountManager : DataManager<AccountManager>
    {
        public static UserGroup DefaultUserGroup = new UserGroup(new UserGroupData() { Id = 0, IsGameMaster = false, Name = "Default", Role = RoleEnum.Player});

        private Dictionary<int, UserGroup> m_userGroups;
        private readonly ConcurrentDictionary<int, Character> m_savingCharacters = new ConcurrentDictionary<int, Character>(); 

        public override void Initialize()
        {
            IPCAccessor.Instance.Granted += accessor =>
            {
                if (m_userGroups == null)
                    WorldServer.Instance.IOTaskPool.ExecuteInContext(LoadUserGroups);
            };

            base.Initialize();
        }

        public void LoadUserGroups()
        {
            if (!IPCAccessor.Instance.IsConnected)
            {
                throw new Exception("IPC not connected");
            }

            var ev = new ManualResetEvent(false);
            IList<UserGroupData> groups = null;
            IPCErrorMessage errorMsg = null;
            IPCAccessor.Instance.SendRequest<GroupsListMessage>(new GroupsRequestMessage(), reply =>
            {
                groups = reply.Groups;
                ev.Set();
            }, error =>
            {
                errorMsg = error;
                ev.Set();
            });
            ev.WaitOne();

            if (groups == null)
                throw new Exception(string.Format("Cannot load groups : {0}", errorMsg.Message));

            m_userGroups = groups.Select(x => new UserGroup(x)).ToDictionary(x => x.Id, x => x);
        }

        public UserGroup GetGroupOrDefault(int id)
        {
            UserGroup group;
            return m_userGroups.TryGetValue(id, out group) ? group : DefaultUserGroup;
        }

        public WorldAccount CreateWorldAccount(WorldClient client)
        {
            /* Create WorldAccount */
            var worldAccount = new WorldAccount
            {
                Id = client.Account.Id,
                Nickname = client.Account.Nickname,
            };
            Database.Insert(worldAccount);

            return worldAccount;
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WorldAccount FindById(int id)
        {
            return Database.FirstOrDefault<WorldAccount>(string.Format(WorldAccountRelator.FetchById, id));
        }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <returns></returns>
        public WorldAccount FindByNickname(string nickname)
        {
            return Database.FirstOrDefault<WorldAccount>(WorldAccountRelator.FetchByNickname, nickname);
        }

        public bool DoesExist(int id)
        {
            return Database.ExecuteScalar<bool>(string.Format("SELECT EXISTS(SELECT 1 FROM accounts WHERE Id={0})", id));
        }

        // block this account 
        public void BlockAccount(WorldAccount account, Character character)
        {
            m_savingCharacters.TryAdd(account.Id, character);
        }

        public void UnBlockAccount(WorldAccount account)
        {
            Character dummy;
            m_savingCharacters.TryRemove(account.Id, out dummy);
        }

        public bool IsAccountBlocked(int accountId, out Character character)
        {
            return m_savingCharacters.TryGetValue(accountId, out character);
        }
    }
}
