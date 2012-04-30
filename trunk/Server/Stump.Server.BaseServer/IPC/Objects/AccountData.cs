using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.IPC.Objects
{
    /// <summary>
    /// Represents a serialized Account
    /// </summary>
    [DataContract]
    public class AccountData
    {
        [DataMember]
        private List<PlayableBreedEnum> m_breeds;

        [DataMember]
        public uint Id
        {
            get;
            set;
        }

        [DataMember]
        public string Login
        {
            get;
            set;
        }

        [DataMember]
        public string PasswordHash
        {
            get;
            set;
        }

        [DataMember]
        public string Nickname
        {
            get;
            set;
        }

        [DataMember]
        public RoleEnum Role
        {
            get;
            set;
        }

        [DataMember]
        public string Ticket
        {
            get;
            set;
        }

        [DataMember]
        public string SecretQuestion
        {
            get;
            set;
        }

        [DataMember]
        public string SecretAnswer
        {
            get;
            set;
        }

        [DataMember]
        public string Lang
        {
            get;
            set;
        }

        [DataMember]
        public string Email
        {
            get;
            set;
        }

        [DataMember]
        public DateTime CreationDate
        {
            get;
            set;
        }

        [DataMember]
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
                    m_breeds.AddRange(Enum.GetValues(typeof (PlayableBreedEnum)).Cast<PlayableBreedEnum>().
                                          Where(breed => CanUseBreed((int) breed)));
                }

                return m_breeds;
            }
            set { BreedFlags = (uint) value.Aggregate(0, (current, breedEnum) => current | (1 << ((int) breedEnum - 1))); }
        }

        [DataMember]
        public IList<uint> CharactersId
        {
            get;
            set;
        }

        [DataMember]
        public int DeletedCharactersCount
        {
            get;
            set;
        }

        [DataMember]
        public DateTime LastDeletedCharacterDate
        {
            get;
            set;
        }

        // we ignore connection[0] cause it's the actual one
        public DateTime? LastConnection
        {
            get { return Connections.Length > 1 ? Connections[1].Key : (DateTime?)null; }
        }

        public bool FirstConnection
        {
            get { return Connections.Length == 1; }
        }

        public string LastConnectionIp
        {
            get { return Connections.Length > 1 ? Connections[1].Value : null; }
        }

        [DataMember]
        public KeyValuePair<DateTime, string>[] Connections
        {
            get;
            set;
        }

        public bool IsSubscribe
        {
            get { return SubscriptionEndDate > DateTime.Now; }
        }

        [DataMember]
        public DateTime SubscriptionEndDate
        {
            get;
            set;
        }

        public bool IsBanned
        {
            get { return BanEndDate > DateTime.Now; }
        }

        [DataMember]
        public DateTime BanEndDate
        {
            get;
            set;
        }

        [DataMember]
        public string BanReason
        {
            get;
            set;
        }

        [DataMember]
        public int Tokens
        {
            get;
            set;
        }

        [DataMember]
        public DateTime? LastVote
        {
            get;
            set;
        }

        public bool CanUseBreed(int breedId)
        {
            if (breedId <= 0)
                return false;

            int flag = (1 << (breedId - 1));
            return (BreedFlags & flag) == flag;
        }
    }
}