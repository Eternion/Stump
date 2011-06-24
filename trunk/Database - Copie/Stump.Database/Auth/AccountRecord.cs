
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Core.Extensions;
using Stump.Database.AuthServer.World;
using Stump.Database.Types;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.AuthServer
{
    [Serializable]
    [ActiveRecord("accounts")]
    public sealed class AccountRecord : AuthBaseRecord<AccountRecord>
    {

        private string m_login = "";
        private IList<WorldCharacterRecord> m_characters;
        private IList<DeletedWorldCharacterRecord> m_deletedCharacters;
        private IList<ConnectionRecord> m_connections;
        private IList<SubscriptionRecord> m_subscriptions;
        private IList<SanctionRecord> m_givenSanctions;
        private IList<SanctionRecord> m_sanctions;
        private List<PlayableBreedEnum> m_breeds;

        public AccountRecord()
        {
            CreationDate = DateTime.Now;
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


        [HasMany(typeof(WorldCharacterRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<WorldCharacterRecord> Characters
        {
            get { return m_characters ?? new List<WorldCharacterRecord>(); }
            set { m_characters = value; }
        }

        [HasMany(typeof(DeletedWorldCharacterRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<DeletedWorldCharacterRecord> DeletedCharacters
        {
            get { return m_deletedCharacters ?? new List<DeletedWorldCharacterRecord>(); }
            set { m_deletedCharacters = value; }
        }

        [HasMany(typeof(ConnectionRecord))]
        public IList<ConnectionRecord> Connections
        {
            get { return m_connections ?? new List<ConnectionRecord>(); }
            set { m_connections = value; }
        }

        [HasMany(typeof(SubscriptionRecord))]
        public IList<SubscriptionRecord> Subscriptions
        {
            get { return m_subscriptions ?? new List<SubscriptionRecord>(); }
            set { m_subscriptions = value; }
        }

        [HasMany(typeof(SanctionRecord))]
        public IList<SanctionRecord> GivenSanctions
        {
            get { return m_givenSanctions ?? new List<SanctionRecord>(); }
            set { m_givenSanctions = value; }
        }

        [HasMany(typeof(SanctionRecord))]
        public IList<SanctionRecord> Sanctions
        {
            get { return m_sanctions ?? new List<SanctionRecord>(); }
            set { m_sanctions = value; }
        }


        public bool CanUseBreed(int breedId)
        {
            return (DbAvailableBreeds >> breedId) % 2 == 1;
        }

        public byte GetCharactersCountByWorld(int worldId)
        {
            return (byte)Characters.Where(entry => entry.World.Id == worldId).Count();
        }

        public IEnumerable<uint> GetWorldCharactersId(int worldId)
        {
            return Characters.Where(c => c.World.Id == worldId).Select(c => c.CharacterId);
        }

        public ConnectionRecord LastConnection
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

        public static AccountRecord FindAccountById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static AccountRecord FindAccountByLogin(string login)
        {
            return FindOne(Restrictions.Eq("Login", login.ToLower()));
        }

        public static AccountRecord FindAccountByNickname(string nickname)
        {
            return FindOne(Restrictions.Eq("Nickname", nickname));
        }

        public static AccountRecord FindAccountByTicket(string ticket)
        {
            return FindAll().First(a => a.Ticket == ticket);
        }

        public static AccountRecord[] FindAccountsByEmail(string email)
        {
            return FindAll(Restrictions.Eq("Email", email));
        }

        public static AccountRecord[] FindAccountsByLastIp(string lastIp)
        {
            return FindAll(Restrictions.Eq("LastIp", lastIp));
        }

        public static AccountRecord[] FindAccountsByRole(RoleEnum role)
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