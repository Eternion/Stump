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
        #region Fields

        private uint m_id;
        private string m_name = "";

        #endregion

        /// <summary>
        ///   Account's Identifier
        /// </summary>
        /// <value>The id.</value>
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        ///   Account's Name
        /// </summary>
        /// <value>The name.</value>
        [Property("Login", NotNull = true, Length = 25)]
        public string Login
        {
            get { return m_name.ToLower(); }
            set { m_name = value.ToLower(); }
        }

        /// <summary>
        ///   Account's password.
        /// </summary>
        /// <value>The password.</value>
        [Property("Password", NotNull = true, Length = 25)]
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        ///   Time of account creation.
        /// </summary>
        /// <value>The created at.</value>
        [Property("CreatedAt", Default = "")]
        public DateTime CreatedAt
        {
            get;
            set;
        }

        /// <summary>
        ///   Email address bound to account.
        /// </summary>
        /// <value>The email.</value>
        [Property("Email")]
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the master account.
        /// </summary>
        /// <value>The master account.</value>
        [Property("Nickname", NotNull = true)]
        public string Nickname
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the question.
        /// </summary>
        /// <value>The question.</value>
        [Property("SecretQuestion", NotNull = true)]
        public string SecretQuestion
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the secret answer
        /// </summary>
        /// <value>The secret answer.</value>
        [Property("SecretAnswer", NotNull = true)]
        public string SecretAnswer
        {
            get;
            set;
        }

        /// <summary>
        ///   Last IP this account logged in from.
        /// </summary>
        /// <value>The last IP.</value>
        [Property("LastIP")]
        public string LastIP
        {
            get;
            set;
        }

        /// <summary>
        ///   Time of last login.
        /// </summary>
        /// <value>The last login.</value>
        [Property("LastLogin")]
        public DateTime LastLogin
        {
            get;
            set;
        }

        /// <summary>
        ///   Last selected server
        /// </summary>
        [Property("LastServer")]
        public int LastServer
        {
            get;
            set;
        }

        /// <summary>
        ///   Represents account's role. Player/Admin etc.
        /// </summary>
        /// <value>The role.</value>
        [Property("Role")]
        public RoleEnum Role
        {
            get;
            set;
        }

        /// <summary>
        ///   Date when the registration ends
        /// </summary>
        /// <value>The End Registration date.</value>
        [Property("RegistrationDate")]
        public DateTime? RegistrationDate
        {
            get;
            set;
        }

        /// <summary>
        ///   Whether the account is banned or not.
        /// </summary>
        /// <value><c>true</c> if banned; otherwise, <c>false</c>.</value>
        [Property("Banned")]
        public bool Banned
        {
            get;
            set;
        }

        /// <summary>
        ///   Time until the account is unbanned (if not permanent). If null, it's
        ///   a permanent ban.
        /// </summary>
        /// <value>The ban time.</value>
        [Property("BanDate")]
        public DateTime? BanDate
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this account is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [Property("Active")]
        public bool Active
        {
            get;
            set;
        }

        /// <summary>
        ///   Gets or sets the ticket.
        /// </summary>
        /// <value>The ticket.</value>
        [Property("Ticket")]
        public string Ticket
        {
            get;
            set;
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
        public double GetRegistrationRemainingTime()
        {
            if (RegistrationDate.HasValue)
            {
                double time = RegistrationDate.Value.Subtract(DateTime.Now).TotalMilliseconds;
                return time > 0 ? time : 0;
            }
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
            if (BanDate >= DateTime.Now)
            {
                Banned = false;
                BanDate = null;
                UpdateAndFlush();

                return true;
            }
            return false;
        }

    }
}