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
using System.Collections.Generic;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.Database;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.AuthServer.Accounts
{
    public static class AccountManager
    {
        /// <summary>
        /// List of available breeds
        /// </summary>
        [Variable]
        public static List<BreedEnum> AvailableBreeds = new List<BreedEnum>
            {
                BreedEnum.Feca,
                BreedEnum.Osamodas,
                BreedEnum.Enutrof,
                BreedEnum.Sram,
                BreedEnum.Xelor,
                BreedEnum.Ecaflip,
                BreedEnum.Eniripsa,
                BreedEnum.Iop,
                BreedEnum.Cra,
                BreedEnum.Sadida,
                BreedEnum.Sacrieur,
                BreedEnum.Pandawa,
                BreedEnum.Roublard,
                //BreedEnum.Zobal,
            };

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static bool AccountExist(string accountname)
        {
            return AccountRecord.FindByLogin(accountname) != null;
        }

        public static AccountRecord GetAccountByName(string username)
        {
            return AccountRecord.FindByLogin(username);
        }

        public static AccountRecord GetAccountByTicket(string ticket)
        {
            return AccountRecord.FindByTicket(ticket);
        }

        public static WorldCharacterRecord[] GetCharactersByAccount(uint accountid)
        {
            return WorldCharacterRecord.FindCharactersByAccountId(accountid);
        }

        public static bool IsBanned(string username)
        {
            AccountRecord acc = GetAccountByName(username);

            return acc != null && acc.Banned;
        }

        public static bool CreateAccount(AccountRecord acc)
        {
            if (AccountExist(acc.Login.ToLower()))
                return false;

            acc.SaveAndFlush();

            return true;
        }

        public static bool CreateAccount(string accountname, string password)
        {
            var acc = new AccountRecord
                {
                    Login = accountname,
                    Password = password,
                    Nickname = accountname,
                    SecretQuestion = "?",
                    SecretAnswer = "!",
                    AvailableBreeds = AvailableBreeds,
                };

            return CreateAccount(acc);
        }

        public static bool DeleteAccount(AccountRecord account)
        {
            if (account == null)
            {
                return false;
            }

            account.DeleteAndFlush();

            return true;
        }
    }
}