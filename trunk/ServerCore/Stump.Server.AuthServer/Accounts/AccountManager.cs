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
        public static List<PlayableBreedEnum> AvailableBreeds = new List<PlayableBreedEnum>
            {
                PlayableBreedEnum.Feca,
                PlayableBreedEnum.Osamodas,
                PlayableBreedEnum.Enutrof,
                PlayableBreedEnum.Sram,
                PlayableBreedEnum.Xelor,
                PlayableBreedEnum.Ecaflip,
                PlayableBreedEnum.Eniripsa,
                PlayableBreedEnum.Iop,
                PlayableBreedEnum.Cra,
                PlayableBreedEnum.Sadida,
                PlayableBreedEnum.Sacrieur,
                PlayableBreedEnum.Pandawa,
                PlayableBreedEnum.Roublard,
                //BreedEnum.Zobal,
            };

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public static bool CreateAccount(AccountRecord acc)
        {
            if (AccountRecord.FindAccountByLogin(acc.Login.ToLower())!=null)
                return false;

            acc.SaveAndFlush();

            return true;
        }

        public static bool DeleteAccount(AccountRecord account)
        {
            if (account == null)
                return false;
            
            account.DeleteAndFlush();

            return true;
        }

    }
}