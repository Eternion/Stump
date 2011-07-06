using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.IPC.Objects
{
    /// <summary>
    /// Represents a serialized Account
    /// </summary>
    public class AccountData
    {
        private List<PlayableBreedEnum> m_breeds;

        public uint Id
        {
            get;
            set;
        }

        public string Login
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Nickname
        {
            get;
            set;
        }

        public RoleEnum Role
        {
            get;
            set;
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

        public uint BreedFlags
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
                BreedFlags = (uint)value.Aggregate(0, (current, breedEnum) => current | ( 1 << (int)breedEnum ));
            }
        }

        public IList<uint> CharactersId
        {
            get;
            set;
        }

        public int DeletedCharactersCount
        {
            get;
            set;
        }

        public DateTime LastConnection
        {
            get;
            set;
        }

        public string LastConnectionIp
        {
            get;
            set;
        }

        public bool IsSubscribe
        {
            get { return SubscriptionEndDate > DateTime.Now; }
        }

        public DateTime SubscriptionEndDate
        {
            get;
            set;
        }

        public bool IsBanned
        {
            get { return BanEndDate > DateTime.Now; }
        }

        public DateTime BanEndDate
        {
            get;
            set;
        }

        public string BanReason
        {
            get;
            set;
        }

        public bool CanUseBreed(int breedId)
        {
            return ( BreedFlags & (1 << breedId) ) == 1;
        }
    }
}