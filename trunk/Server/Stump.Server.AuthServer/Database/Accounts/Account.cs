using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.IPC.Objects;

namespace Stump.Server.AuthServer.Database
{
    public class AccountConfiguration : EntityTypeConfiguration<Account>
    {
        public AccountConfiguration()
        {
            ToTable("accounts");

            Ignore(x => x.Role);
            Ignore(x => x.AvailableBreeds);

            HasMany(x => x.Connections);
            HasMany(x => x.WorldCharacters);
            HasMany(x => x.Sanctions);
            HasMany(x => x.Subscriptions);
            HasMany(x => x.WorldDeletedCharacters);
        }
    }

    public partial class Account
    {
        private List<PlayableBreedEnum> m_availableBreeds;

        #region Record Properties

        private long m_availableBreedsFlag;

        public Account()
        {
            AvailableBreedsFlag = 16383;
            Subscriptions = new HashSet<Subscription>();
            Sanctions = new HashSet<Sanction>();
            WorldCharacters = new HashSet<WorldCharacter>();
            WorldDeletedCharacters = new HashSet<WorldCharacterDeleted>();
            Connections = new HashSet<Connection>();
        }

        // Primitive properties
       
        public int Id
        {
            get;
            set;
        }

        public string Login
        {
            get;
            set;
        }

        public string PasswordHash
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        private int RoleAsInt
        {
            get;
            set;
        }

        public long AvailableBreedsFlag
        {
            get { return m_availableBreedsFlag; }
            set
            {
                m_availableBreedsFlag = value;
                m_availableBreeds = new List<PlayableBreedEnum>();
                m_availableBreeds.AddRange(Enum.GetValues(typeof (PlayableBreedEnum)).Cast<PlayableBreedEnum>().
                                               Where(IsBreedAvailable));
            }
        }

        public string Ticket
        {
            get;
            set;
        }

        public string SecretQuestion
        {
            get;
            set;
        }

        public string SecretAnswer
        {
            get;
            set;
        }

        public string Lang
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get;
            set;
        }

        public int Tokens
        {
            get;
            set;
        }

        public int NewTokens
        {
            get;
            set;
        }

        public DateTime? LastVote
        {
            get;
            set;
        }

        // Navigation properties

        public virtual ICollection<Subscription> Subscriptions
        {
            get;
            set;
        }

        public virtual ICollection<Sanction> Sanctions
        {
            get;
            set;
        }

        public virtual ICollection<WorldCharacter> WorldCharacters
        {
            get;
            set;
        }

        public virtual ICollection<WorldCharacterDeleted> WorldDeletedCharacters
        {
            get;
            set;
        }

        public virtual ICollection<Connection> Connections
        {
            get;
            set;
        }

        #endregion

        public RoleEnum Role
        {
            get { return (RoleEnum) RoleAsInt; }
            set { RoleAsInt = (byte) value; }
        }

        public List<PlayableBreedEnum> AvailableBreeds
        {
            get
            {
                if (m_availableBreeds == null)
                {
                    m_availableBreeds = new List<PlayableBreedEnum>();
                    m_availableBreeds.AddRange(Enum.GetValues(typeof (PlayableBreedEnum)).Cast<PlayableBreedEnum>().
                                                   Where(IsBreedAvailable));
                }

                return m_availableBreeds;
            }
            set
            {
                AvailableBreedsFlag =
                    (uint) value.Aggregate(0, (current, breedEnum) => current | (1 << ((int) breedEnum - 1)));
            }
        }

        public AccountData Serialize()
        {
            Sanction strongestSanction = GetStrongestSanction();

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
                           BanEndDate = strongestSanction != null ? strongestSanction.GetEndDate() : default(DateTime),
                           BanReason = strongestSanction != null ? strongestSanction.BanReason : string.Empty,
                           SubscriptionEndDate = GetSubscriptionEndDate(),
                           Connections =
                               Connections.Select(entry => new KeyValuePair<DateTime, string>(entry.Date, entry.Ip)).
                               ToArray(),
                           CharactersId = WorldCharacters.Select(entry => entry.CharacterId).ToList(),
                           DeletedCharactersCount =
                               WorldDeletedCharacters.Count(
                                   entry => DateTime.Now - entry.DeletionDate <= TimeSpan.FromDays(1)),
                           Tokens = Tokens,
                           LastVote = LastVote,
                       };
        }

        public Connection GetLastConnection()
        {
            if (Connections.Count == 0)
                return null;

            return Connections.OrderByDescending(entry => entry.Date).FirstOrDefault();
        }

        public Sanction GetStrongestSanction()
        {
            if (Sanctions.Count == 0)
                return null;

            return Sanctions.OrderByDescending(entry => entry.GetEndDate()).First();
        }

        public DateTime GetSubscriptionEndDate()
        {
            if (Sanctions.Count == 0)
                return DateTime.MinValue;

            return DateTime.Now + new TimeSpan(Subscriptions.Sum(entry => entry.GetRemainingTime().Ticks));
        }

        public TimeSpan GetBanRemainingTime()
        {
            if (Sanctions.Count == 0)
                return TimeSpan.Zero;

            return Sanctions.Max(entry => entry.GetRemainingTime());
        }

        public void RemoveOldestConnection()
        {
            if (Connections.Count == 0)
                return;

            Connection oldestConnection = Connections.OrderBy(entry => entry.Date).First();

            Connections.Remove(oldestConnection);
        }

        public bool IsBreedAvailable(PlayableBreedEnum breed)
        {
            if ((int) breed <= 0)
                return false;

            int flag = (1 << ((int) breed - 1));
            return (AvailableBreedsFlag & flag) == flag;
        }

        public sbyte GetCharactersCountByWorld(int worldId)
        {
            return (sbyte) WorldCharacters.Count(entry => entry.WorldId == worldId);
        }

        public IEnumerable<int> GetWorldCharactersId(int worldId)
        {
            return WorldCharacters.Where(c => c.WorldId == worldId).Select(c => c.CharacterId);
        }
    }
}