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
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [Serializable]
    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("accounts")]
    public sealed class AccountRecord : ActiveRecordBase<AccountRecord>
    {

        private string m_name;
        [Field("Ticket")]
        public string ticket;

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Login", NotNull = true, Length = 25)]
        public string Login
        {
            get { return m_name.ToLower(); }
            set { m_name = value.ToLower(); }
        }

        [Property("Password", NotNull = true, Length = 25)]
        public string Password
        {
            get;
            set;
        }

        [Property("Nickname", NotNull = false, Length = 25)]
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

        [Property("AvailableBreeds", Default = "8191")]
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
        public DateTime? LastConnection
        {
            get;
            set;
        }

        [Property("LastIp", NotNull = false, Length = 15)]
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

        [Property("Email", Length = 50, NotNull = true)]
        public string Email
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

        [Property("SubscriptionEndDate", NotNull = false)]
        public DateTime? SubscriptionEndDate
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

        [HasMany(typeof(WorldCharacterRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<WorldCharacterRecord> Characters
        {
            get;
            set;
        }

        [HasMany(typeof(BanRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<BanRecord> Bans
        {
            get;
            set;
        }

        [HasMany(typeof(DeletedWorldCharacterRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<DeletedWorldCharacterRecord> DeletedWorldCharacters
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

        public bool Banned
        {
            set; get; }

        public DateTime? BanDate { get; set; }

        public static AccountRecord FindById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static AccountRecord FindByLogin(string login)
        {
            return FindOne(Restrictions.Eq("Login", login));
        }

        public static AccountRecord FindByNickname(string nickname)
        {
            return FindOne(Restrictions.Eq("Nickname", nickname));
        }

        public static AccountRecord FindByTicket(string ticket)
        {
            return FindOne(Restrictions.Eq("Ticket", ticket));
        }

    }
}