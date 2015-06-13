using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.BaseServer.IPC.Objects;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Breeds;

namespace FakeClients
{
    public class FakeClientManager : Singleton<FakeClientManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable(true)] 
        public static string AccountName = "FakeClient";
        [Variable(true)] 
        public static string AccountPassword = "FakePassword";

        [Variable(true)]
        public static int FakeUserGroup = 10;

        [Variable(true)]
        public static string AuthAddress = "localhost";

        [Variable(true)]
        public static int AuthPort = 443;

        private ConcurrentList<FakeClient> m_clients = new ConcurrentList<FakeClient>();
        private ConcurrentList<int> m_fakeAccountsId = new ConcurrentList<int>();

        private UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public FakeClientManager()
        {
            IP = AuthAddress;
            Port = AuthPort;
        }

        public string IP
        {
            get;
            private set;
        }

        public int Port
        {
            get;
            private set;
        }

        public IReadOnlyCollection<FakeClient> Clients
        {
            get { return m_clients.AsReadOnly(); }
        }

        [Initialization(InitializationPass.Eighth, Silent = true)]
        public void CheckAccounts()
        {
            IPCAccessor.Instance.Granted += accessor =>
            {
                WorldServer.Instance.IOTaskPool.ExecuteInContext(() =>
                {
                    IPCAccessor.Instance.SendRequest<AccountsAnswerMessage>(
                        new AccountsRequestMessage() {LoginLike = AccountName + "%"}, x =>
                        {
                            if (x.Accounts != null)
                            {
                                foreach (var account in x.Accounts)
                                {
                                    int id;
                                    if (int.TryParse(account.Login.Remove(0, AccountName.Length), out id))
                                        m_fakeAccountsId.Add(id);
                                }
                            }
                        });

                    var usergroup = AccountManager.Instance.GetGroupOrDefault(FakeUserGroup);

                    if (usergroup == AccountManager.DefaultUserGroup)
                    {
                        var data = new UserGroupData()
                        {
                            Id = FakeUserGroup,
                            Name = "FakeClient",
                            IsGameMaster = true,
                            Role = RoleEnum.Moderator,
                            Servers = new[] {WorldServer.ServerInformation.Id},
                            Commands = new string[0]
                        };

                        IPCAccessor.Instance.SendRequest<GroupAddResultMessage>(new GroupAddMessage(data), x =>
                        {
                            FakeUserGroup = x.UserGroup.Id;
                            AccountManager.Instance.AddUserGroup(new UserGroup(x.UserGroup));
                            Plugin.CurrentPlugin.Config.Save();
                        });
                    }
                });
            };
        }


        public FakeClient AddAndConnectClient()
            {
            var client = new FakeClient(m_idProvider.Pop());
            m_clients.Add(client);

            if (!m_fakeAccountsId.Contains(client.Id))
            {
                var account = new AccountData()
                {
                    Login = AccountName + client.Id,
                    PasswordHash = AccountPassword.GetMD5(),
                    Nickname = AccountName + client.Id,
                    UserGroupId = FakeUserGroup,
                    AvailableBreeds = BreedManager.AvailableBreeds,
                    SecretQuestion = "fakeclient",
                    SecretAnswer = "fakeclient",
                    Lang = "fr",
                };

                var msg = new CreateAccountMessage();
                msg.Account = account;

                IPCAccessor.Instance.SendRequest(msg, x =>
                {
                    m_fakeAccountsId.Add(client.Id);
                    client.Connect(IP, Port);
                    client.Disconnected += OnClientDisconnected;
                }, x =>
                {
                    client.Disconnect();
                    m_clients.Remove(client);
                });
            }
            else
            {
                client.Connect(IP, Port);
                client.Disconnected += OnClientDisconnected;
            }


            return client;
        }
        public IEnumerable<FakeClient> AddAndConnectClients(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return AddAndConnectClient();
            }
        }

        private void OnClientDisconnected(FakeClient obj, bool planned)
        {
            if (!obj.ConnectingToAuth)
                m_clients.Remove(obj);
        }


    }
}