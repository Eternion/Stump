// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.Types;
using Stump.Database.WorldServer.StartupAction;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.AuthServer
{
    [Serializable]
    [ActiveRecord("accounts")]
    public sealed class AccountRecord : AuthRecord<AccountRecord>
    {

        private string m_login = "";
        private IList<WorldCharacterRecord> m_characters;
        private IList<DeletedWorldCharacterRecord> m_deletedCharacters;

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

        [Property("Active", NotNull = true, Default = "1")]
        public bool Active
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

        [Property("LastServer", NotNull = false)]
        public int? LastServer
        {
            get;
            set;
        }

        [Property("LastConnection", NotNull = false)]
        public DateTime LastConnection
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

        [Property("LastIp", NotNull = false)]
        public string LastIp
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

        [Property("Email", NotNull = true)]
        public string Email
        {
            get;
            set;
        }

        [Property("SubscriptionEndDate", NotNull = false)]
        public DateTime? SubscriptionEndDate
        {
            get;
            set;
        }

        [Property("Banned", NotNull = true, Default = "0")]
        public bool Banned
        {
            get;
            set;
        }

        [Property("BanEndDate", NotNull=false)]
        public DateTime? BanEndDate
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
                var breeds = new List<PlayableBreedEnum>(14);
                foreach (PlayableBreedEnum breed in Enum.GetValues(typeof(PlayableBreedEnum)))
                {
                    if (((DbAvailableBreeds >> (byte)breed) % 2 == 1))
                        breeds.Add(breed);
                }
                return breeds;
            }
            set
            {
                DbAvailableBreeds = (uint)value.Aggregate(0, (current, breedEnum) => current | (1 << (int)breedEnum));
            }
        }

        public double SubscriptionRemainingTime
        {
            get
            {
                if (SubscriptionEndDate.HasValue)
                {
                    double time = SubscriptionEndDate.Value.Subtract(DateTime.Now).TotalMilliseconds;
                    return time > 0 ? time : 0;
                }
                return 0;
            }
        }

        public double BanRemainingTime
        {
            get
            {
                if (BanEndDate.HasValue)
                {
                    double time = BanEndDate.Value.Subtract(DateTime.Now).TotalSeconds;
                    return time > 0 ? time : 0;
                }
                return 0;
            }
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

        public void UpdateBanStatus()
        {
            /* Update if we have passed BanEndDate */
            if (Banned && BanEndDate.HasValue && BanRemainingTime == 0)
            {
                Banned = false;
                BanEndDate = null;
                SaveAndFlush();
            }
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