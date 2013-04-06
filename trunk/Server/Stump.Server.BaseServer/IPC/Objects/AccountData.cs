using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using ProtoBuf;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.BaseServer.IPC.Objects
{
    /// <summary>
    /// Represents a serialized Account
    /// </summary>
    [ProtoContract]
    public class AccountData
    {
        private List<PlayableBreedEnum> m_breeds;

        [ProtoMember(1)]
        public int Id
        {
            get;
            set;
        }

        [ProtoMember(2)]
        public string Login
        {
            get;
            set;
        }

        [ProtoMember(3)]
        public string PasswordHash
        {
            get;
            set;
        }

        [ProtoMember(4)]
        public string Nickname
        {
            get;
            set;
        }

        [ProtoMember(5)]
        public RoleEnum Role
        {
            get;
            set;
        }

        [ProtoMember(6)]
        public string Ticket
        {
            get;
            set;
        }

        [ProtoMember(7)]
        public string SecretQuestion
        {
            get;
            set;
        }

        [ProtoMember(8)]
        public string SecretAnswer
        {
            get;
            set;
        }

        [ProtoMember(9)]
        public string Lang
        {
            get;
            set;
        }

        [ProtoMember(10)]
        public string Email
        {
            get;
            set;
        }

        [ProtoMember(11)]
        public DateTime CreationDate
        {
            get;
            set;
        }

        [ProtoMember(12)]
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

        private IList<int> m_charactersId = new List<int>();

        [ProtoMember(13)]
        public IList<int> CharactersId
        {
            get { return m_charactersId; }
            set { m_charactersId = value; }
        }

        [ProtoMember(14)]
        public int DeletedCharactersCount
        {
            get;
            set;
        }

        [ProtoMember(15)]
        public DateTime LastDeletedCharacterDate
        {
            get;
            set;
        }

        [ProtoMember(16)]
        public DateTime? LastConnection
        {
            get;
            set;
        }

        [ProtoMember(17)]
        public string LastConnectionIp
        {
            get;
            set;
        }

        public bool IsSubscribe
        {
            get { return SubscriptionEndDate > DateTime.Now; }
        }

        [ProtoMember(18)]
        public DateTime SubscriptionEndDate
        {
            get;
            set;
        }

        public bool IsBanned
        {
            get { return BanEndDate > DateTime.Now; }
        }

        [ProtoMember(19)]
        public DateTime? BanEndDate
        {
            get;
            set;
        }

        [ProtoMember(20)]
        public string BanReason
        {
            get;
            set;
        }

        [ProtoMember(21)]
        public int Tokens
        {
            get;
            set;
        }

        [ProtoMember(22)]
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