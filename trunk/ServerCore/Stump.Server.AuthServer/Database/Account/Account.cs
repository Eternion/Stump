using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database.World;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.AuthServer.Database.Account
{
    [Serializable]
    [ActiveRecord("accounts")]
    public sealed class Account : AuthBaseRecord<Account>
    {

        private string m_login = "";
        private IList<WorldCharacter> m_characters;
        private IList<DeletedWorldCharacter> m_deletedCharacters;
        private IList<ConnectionLog> m_connections;
        private IList<SubscriptionLog> m_subscriptions;
        private IList<Sanction> m_givenSanctions;
        private IList<Sanction> m_sanctions;
        private List<PlayableBreedEnum> m_breeds;

        public Account()
        {
            CreationDate = DateTime.Now;
        }

        public Account(AccountData accountData)
        {
            CreationDate = DateTime.Now;

            // todo : blabla
        }

        public AccountData Serialize()
        {
            return new AccountData();
        }

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Login", NotNull = true, Length = 19)]
        public string Login
        {
            get { return m_login.ToLower(); }
            set { m_login = value.ToLower(); }
        }

        [Property("Password", NotNull = true, Length = 49)]
        public string Password
        {
            get;
            set;
        }

        [Property("Nickname", NotNull = true, Length = 29)]
        public string Nickname
        {
            get;
            set;
        }

        [Property("Role", NotNull = true, Default = "1")]
        public RoleEnum Role
        {
            get;
            set;
        }

        [Property("AvailableBreeds", NotNull = true, Default = "8191")]
        public uint DbAvailableBreeds
        {
            get;
            set;
        }

        [Property("Ticket", NotNull = false)]
        public string Ticket
        {
            get;
            set;
        }

        [Property("SecretQuestion", NotNull = true)]
        public string SecretQuestion
        {
            get;
            set;
        }

        [Property("SecretAnswer", NotNull = true)]
        public string SecretAnswer
        {
            get;
            set;
        }

        [Property("Lang", NotNull = true)]
        public string Lang
        {
            get;
            set;
        }

        [Property("Email", NotNull = true)]
        public string Email
        {
            get;
            set;
        }

        [Property("CreationDate",NotNull=true)]
        public DateTime CreationDate
        {
            get;
            set;
        }

        public List<PlayableBreedEnum> AvailableBreeds
        {
            get
            {
                if (m_breeds == null)
                {
                    m_breeds = new List<PlayableBreedEnum>();
                    m_breeds.AddRange(Enum.GetValues(typeof(PlayableBreedEnum)).Cast<PlayableBreedEnum>().
                        Where(breed => CanUseBreed((int)breed)));
                }

                return m_breeds;
            }
            set
            {
                DbAvailableBreeds = (uint)value.Aggregate(0, (current, breedEnum) => current | ( 1 << (int)breedEnum ));
            }
        }

        [HasMany(typeof(WorldCharacter), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<WorldCharacter> Characters
        {
            get { return m_characters ?? new List<WorldCharacter>(); }
            set { m_characters = value; }
        }

        [HasMany(typeof(DeletedWorldCharacter), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<DeletedWorldCharacter> DeletedCharacters
        {
            get { return m_deletedCharacters ?? new List<DeletedWorldCharacter>(); }
            set { m_deletedCharacters = value; }
        }

        [HasMany(typeof(ConnectionLog))]
        public IList<ConnectionLog> Connections
        {
            get { return m_connections ?? new List<ConnectionLog>(); }
            set { m_connections = value; }
        }

        [HasMany(typeof(SubscriptionLog))]
        public IList<SubscriptionLog> Subscriptions
        {
            get { return m_subscriptions ?? new List<SubscriptionLog>(); }
            set { m_subscriptions = value; }
        }

        [HasMany(typeof(Sanction))]
        public IList<Sanction> GivenSanctions
        {
            get { return m_givenSanctions ?? new List<Sanction>(); }
            set { m_givenSanctions = value; }
        }

        [HasMany(typeof(Sanction))]
        public IList<Sanction> Sanctions
        {
            get { return m_sanctions ?? new List<Sanction>(); }
            set { m_sanctions = value; }
        }


        public bool CanUseBreed(int breedId)
        {
            return ( DbAvailableBreeds & ( 1 << breedId ) ) == 1;
        }

        public byte GetCharactersCountByWorld(int worldId)
        {
            return (byte)Characters.Where(entry => entry.World.Id == worldId).Count();
        }

        public IEnumerable<uint> GetWorldCharactersId(int worldId)
        {
            return Characters.Where(c => c.World.Id == worldId).Select(c => c.CharacterId);
        }

        public ConnectionLog LastConnection
        {          
            get { return Connections.MaxOf(c => c.Date); }
        }

        public uint SubscriptionRemainingTime
        {
            get
            {
                var time = new TimeSpan();

                for (var i = 0; i < Subscriptions.Count; i++)
                {
                    var diff = Subscriptions[i].EndDate.Subtract(DateTime.Now);

                    if (diff > TimeSpan.Zero)
                        time += diff;
                    else if (i < Subscriptions.Count - 1)
                    {
                        diff = Subscriptions[i].EndDate.Subtract(Subscriptions[i + 1].BuyDate);

                        if (diff > TimeSpan.Zero)
                            Subscriptions[i + 1].Duration += diff;
                    }
                }
                return (uint)time.TotalMilliseconds;
            }
        }

        public uint BanRemainingTime
        {
            get
            {
                if (Sanctions.Count == 0) return 0;
                return (uint)DateTime.Now.Subtract( Sanctions.Max(s => s.EndDate)).TotalSeconds;
            }
        }

        public void RemoveOldestConnection()
        {
            var olderConn = Connections.MinOf(c => c.Date);
            olderConn.Delete();
        }

        public static Account FindAccountById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static Account FindAccountByLogin(string login)
        {
            return FindOne(Restrictions.Eq("Login", login.ToLower()));
        }

        public static Account FindAccountByNickname(string nickname)
        {
            return FindOne(Restrictions.Eq("Nickname", nickname));
        }

        public static Account FindAccountByTicket(string ticket)
        {
            return FindAll().First(a => a.Ticket == ticket);
        }

        public static Account[] FindAccountsByEmail(string email)
        {
            return FindAll(Restrictions.Eq("Email", email));
        }

        public static Account[] FindAccountsByLastIp(string lastIp)
        {
            return FindAll(Restrictions.Eq("LastIp", lastIp));
        }

        public static Account[] FindAccountsByRole(RoleEnum role)
        {
            return FindAll(Restrictions.Eq("Role", role));
        }

        public static bool LoginExist(string login)
        {
            return Exists(Restrictions.Eq("Login", login.ToLower()));
        }

        public static bool NicknameExist(string nickname)
        {
            return Exists(Restrictions.Eq("Nickname", nickname));
        }

    }
}