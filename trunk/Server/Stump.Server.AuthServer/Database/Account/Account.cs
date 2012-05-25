using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Database.World;
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
        private IList<Sanction> m_sanctions;
        private List<PlayableBreedEnum> m_availableBreeds;

        public Account()
        {
            CreationDate = DateTime.Now;
        }

        public AccountData Serialize()
        {
            var strongestSanction = StrongestSanction;

            return new AccountData
                       {
                           Id = Id,
                           Login = Login,
                           PasswordHash = PasswordHash,
                           Nickname = Nickname,
                           Role = Role,
                           AvailableBreeds = AvailableBreeds,
                           Ticket = Ticket,
                           SecretQuestion = SecretQuestion,
                           SecretAnswer = SecretAnswer,
                           Lang = Lang,
                           Email = Email,
                           CreationDate = CreationDate,
                           BanEndDate = strongestSanction != null ? StrongestSanction.EndDate : default(DateTime),
                           BanReason = strongestSanction != null ? strongestSanction.BanReason : string.Empty,
                           SubscriptionEndDate = DateTime.Now + TimeSpan.FromSeconds(SubscriptionRemainingTime),
                           Connections = Connections.Select(entry => new KeyValuePair<DateTime, string>(entry.Date, entry.Ip)).ToArray(),
                           CharactersId = Characters.Select(entry => entry.CharacterId).ToList(),
                           DeletedCharactersCount = DeletedCharacters.Count(entry => DateTime.Now - entry.DeletionDate <= TimeSpan.FromDays(1)),
                           Tokens = Tokens,
                           LastVote = LastVote,
                       };
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
            get { return m_login; }
            set { m_login = value.ToLower(); }
        }

        [Property("PasswordHash", NotNull = true, Length = 32)]
        public string PasswordHash
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

        private uint m_dbAvailableBreeds;

        [Property("AvailableBreeds", NotNull = true, Default = "16383")] // 16383 = 0011 1111 1111 1111
            public uint DbAvailableBreeds
        {
            get { return m_dbAvailableBreeds; }
            set
            {
                m_dbAvailableBreeds = value;
                m_availableBreeds = null;
            }
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

        [Property("CreationDate", NotNull = true)]
        public DateTime CreationDate
        {
            get;
            set;
        }

        [Property]
        public int Tokens
        {
            get;
            set;
        }

        [Property]
        public int NewTokens
        {
            get;
            set;
        }

        [Property]
        public DateTime? LastVote
        {
            get;
            set;
        }

        [Version]
        public int RecordVersion
        {
            get;
            set;
        }


        public List<PlayableBreedEnum> AvailableBreeds
        {
            get
            {
                if (m_availableBreeds == null)
                {
                    m_availableBreeds = new List<PlayableBreedEnum>();
                    m_availableBreeds.AddRange(Enum.GetValues(typeof (PlayableBreedEnum)).Cast<PlayableBreedEnum>().
                                                   Where(breed => CanUseBreed((int) breed)));
                }

                return m_availableBreeds;
            }
            set
            {
                DbAvailableBreeds = (uint)value.Aggregate(0, (current, breedEnum) => current | ( 1 << ( (int)breedEnum - 1 ) ));
            }
        }

        [HasMany(typeof (WorldCharacter))]
        public IList<WorldCharacter> Characters
        {
            get { return m_characters ?? new List<WorldCharacter>(); }
            set { m_characters = value; }
        }

        [HasMany(typeof (DeletedWorldCharacter))]
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
        public IList<Sanction> Sanctions
        {
            get { return m_sanctions ?? new List<Sanction>(); }
            set { m_sanctions = value; }
        }

        public Sanction StrongestSanction
        {
            get { return Sanctions.Count == 0 ? null : Sanctions.MaxOf(entry => entry.EndDate); }
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
                return (uint) time.TotalMilliseconds;
            }
        }

        public uint BanRemainingTime
        {
            get
            {
                if (Sanctions.Count == 0)
                    return 0;

                var remainingTime = (Sanctions.Max(s => s.EndDate) - DateTime.Now).TotalMinutes;

                if (remainingTime < 0)
                    return 0;

                return (uint) remainingTime;
            }
        }

        public void RemoveOldestConnection()
        {
            Connections.Remove(LastConnection);
            LastConnection.Delete();
        }

        public bool CanUseBreed(int breedId)
        {
            if (breedId <= 0)
                return false;

            int flag = (1 << (breedId - 1));
            return ( DbAvailableBreeds & flag ) == flag;
        }

        public sbyte GetCharactersCountByWorld(int worldId)
        {
            return (sbyte) Characters.Where(entry => entry.World.Id == worldId).Count();
        }

        public IEnumerable<uint> GetWorldCharactersId(int worldId)
        {
            return Characters.Where(c => c.World.Id == worldId).Select(c => c.CharacterId);
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
            return FindOne(Restrictions.Eq("Ticket", ticket));
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
    }
}