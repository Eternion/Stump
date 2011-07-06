using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database.Account;
using Stump.Server.AuthServer.Database.World;
using Stump.Server.AuthServer.IPC;

namespace Stump.Server.AuthServer.Managers
{
    public class AccountManager : Singleton<AccountManager>
    {
        /// <summary>
        /// List of available breeds
        /// </summary>
        [Variable]
        public static List<PlayableBreedEnum> AvailableBreeds = new List<PlayableBreedEnum>
            {
                PlayableBreedEnum.Feca,
                PlayableBreedEnum.Osamodas,
                PlayableBreedEnum.Enutrof,
                PlayableBreedEnum.Sram,
                PlayableBreedEnum.Xelor,
                PlayableBreedEnum.Ecaflip,
                PlayableBreedEnum.Eniripsa,
                PlayableBreedEnum.Iop,
                PlayableBreedEnum.Cra,
                PlayableBreedEnum.Sadida,
                PlayableBreedEnum.Sacrieur,
                PlayableBreedEnum.Pandawa,
                PlayableBreedEnum.Roublard,
                //BreedEnum.Zobal,
            };

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public Account FindAccount(string login)
        {
            return Account.FindAccountByLogin(login);
        }

        public bool LoginExist(string login)
        {
            return Account.Exists(Restrictions.Eq("Login", login.ToLower()));
        }

        public bool NicknameExist(string nickname)
        {
            return Account.Exists(Restrictions.Eq("Nickname", nickname));
        }

        public bool CreateAccount(Account account)
        {
            if (LoginExist(account.Login.ToLower()))
                return false;

            // todo : generate the id to avoid the flush
            account.CreateAndFlush();
            
            return true;
        }

        public bool DeleteAccount(Account account)
        {
            account.Delete();

            return true;
        }

        public WorldCharacter CreateAccountCharacter(Account account, WorldServer world, uint characterId)
        {
            var character = new WorldCharacter
            {
                Account = account,
                World = world,
                CharacterId = characterId
            };

            character.Create();

            return character;
        }

        public bool AddAccountCharacter(Account account, WorldServer world, uint characterId)
        {

            var character = CreateAccountCharacter(account, world, characterId);

            if (account.Characters.Contains(character))
                return false;

            account.Characters.Add(character);
            account.Update();

            return true;
        }

        public DeletedWorldCharacter AddDeletedCharacter(Account account, WorldServer world, uint characterId)
        {
            var character = new DeletedWorldCharacter
                                {
                                    Account = account,
                                    World = world,
                                    CharacterId = characterId
                                };

            character.Create();

            return character;
        }

        public bool DeleteAccountCharacter(Account account, WorldServer world, uint characterId)
        {
            var character = account.Characters.FirstOrDefault(c => c.CharacterId == characterId);

            if (character == null)
                return false;

            account.Characters.Remove(character);
            character.Delete();

            account.DeletedCharacters.Add(AddDeletedCharacter(account, world, characterId));
            account.Save();

            return true;
        }


        public bool DisconnectClientsUsingAccount(Account account)
        {
            var clients = AuthServer.Instance.FindClients(entry => entry.Account != null &&
                entry.Account.Id == account.Id);

            // disconnect clients from auth server
            foreach (var client in clients)
            {
                client.Disconnect();
            }

            if (account.LastConnection != null)
            {
                var lastWorld = account.LastConnection.World;

                // diconnect clients from last game server
                if (IpcServer.Instance.GetIpcClient(lastWorld.Id).DisconnectConnectedAccount(account.Id))
                {
                    return true;
                }
            }

            return clients.Count() > 0;
        }
    }
}