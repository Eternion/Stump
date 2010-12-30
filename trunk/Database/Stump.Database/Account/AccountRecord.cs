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
        private string m_ticket;

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
        public DateTime LastConnection
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

        [Property("CreationDate", NotNull = true, Default = "CURRENT_TIMESTAMP")]
        public DateTime CreationDate
        {
            get;
            set;
        }

        [HasMany(typeof(WorldCharacterRecord))]
        public IList<WorldCharacterRecord> Characters
        {
            get;
            set;
        }

        //[HasMany(typeof(BanRecord))]
        //public IList<BanRecord> Bans
        //{
        //    get;
        //    set;
        //}

        //[HasMany(typeof(DeletedWorldCharacterRecord))]
        //public IList<DeletedWorldCharacterRecord> DeletedWorldCharacters
        //{
        //    get;
        //    set;
        //}

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

        /// <summary>
        ///   Sets account active.
        /// </summary>
        public void SetActive()
        {
            Active = true;
            SaveAndFlush();
        }


        /// <summary>
        ///   Finds the account by his id.
        /// </summary>
        /// <param name = "login">Id of the account.</param>
        /// <returns></returns>
        public static AccountRecord FindById(uint id)
        {
            // Note that we use the property name, _not_ the column name
            return FindByPrimaryKey(id);
        }

        /// <summary>
        ///   Finds the account  by his name.
        /// </summary>
        /// <param name = "login">Name of the account.</param>
        /// <returns></returns>
        public static AccountRecord FindByLogin(string login)
        {
            // Note that we use the property name, _not_ the column name
            return FindOne(Restrictions.Eq("Login", login.ToLower()));
        }

        /// <summary>
        ///   Finds the account by his ticket.
        /// </summary>
        /// <param name = "ticket">The ticket.</param>
        /// <returns></returns>
        public static AccountRecord FindByTicket(string ticket)
        {
            // Note that we use the property name, _not_ the column name
            return FindOne(Restrictions.Eq("Ticket", ticket));
        }

        /// <summary>
        ///   Finds the account by his Masteraccount.
        /// </summary>
        /// <param name = "ticket">The masterAccount.</param>
        /// <returns></returns>
        public static AccountRecord FindByNickname(string nickname)
        {
            // Note that we use the property name, _not_ the column name
            return FindOne(Restrictions.Eq("Nickname", nickname));
        }

        /// <summary>
        ///   Gets fields count
        /// </summary>
        /// <returns></returns>
        public static int GetCount()
        {
            return Count();
        }

        /// <summary>
        /// Get the remaining subscription time
        /// </summary>
        /// <returns>The remaining time in milliseconds</returns>
        public double GetSubscriptionRemainingTime()
        {
            if (SubscriptionEndDate.HasValue)
            {
                double time = SubscriptionEndDate.Value.Subtract(DateTime.Now).TotalMilliseconds;
                return time > 0 ? time : 0;
            }
            return 0;
        }

        /// <summary>
        /// Get the remaining ban time
        /// </summary>
        /// <returns>The remaining time in milliseconds</returns>
        public double GetBanRemainingTime()
        {
            //if (BanDate.HasValue)
            //{
            //    double time = BanDate.Value.Subtract(DateTime.Now).TotalSeconds;
            //    return time > 0 ? time : 0;
            //}
            return 0;
        }

        /// <summary>
        ///   Updates the ban status.
        /// </summary>
        /// <returns></returns>
        public bool UpdateBanStatus()
        {
            // Update banned status.
            // Hmm BanTime ? We might want to change that ! =)
            //if (BanDate >= DateTime.Now)
            //{
            //    Banned = false;
            //    BanDate = null;
            //    UpdateAndFlush();

            //    return true;
            //}
            return false;
        }

        public bool IsBreedAvailable(PlayableBreedEnum breed)
        {
            return (DbAvailableBreeds >> (byte)breed) % 2 == 1;
        }

        public bool IsBreedAvailable(int breedId)
        {
            return (DbAvailableBreeds >> breedId) % 2 == 1;
        }

    }
}