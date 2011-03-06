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
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.Database.AuthServer.World;
using Stump.Database.WorldServer.StartupAction;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.AuthServer.Managers
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


        public static bool CreateAccount(AccountRecord account)
        {
            if (AccountRecord.LoginExist(account.Login.ToLower()))
                return false;

            account.CreateAndFlush();

            return true;
        }

        public static bool DeleteAccount(AccountRecord account)
        {
            account.DeleteAndFlush();

            return true;
        }

        public static WorldCharacterRecord CreateAccountCharacter(AccountRecord account, WorldRecord world, uint characterId)
        {
            var character = new WorldCharacterRecord
            {
                Account = account,
                World = world,
                CharacterId = characterId
            };

            character.CreateAndFlush();

            return character;
        }

        public static DeletedWorldCharacterRecord CreateAccountDeletedCharacter(AccountRecord account, WorldRecord world, uint characterId)
        {
            var character = new DeletedWorldCharacterRecord
            {
                Account = account,
                World = world,
                CharacterId = characterId
            };

            character.CreateAndFlush();

            return character;
        }

        public static bool AddAccountCharacter(AccountRecord account, WorldRecord world, uint characterId)
        {

            var character = CreateAccountCharacter(account, world, characterId);

            if (account.Characters.Contains(character))
                return false;

            account.Characters.Add(character);
            account.SaveAndFlush();

            return true;
        }

        public static bool DeleteAccountCharacter(AccountRecord account, WorldRecord world, uint characterId)
        {
            var character = account.Characters.FirstOrDefault(c => c.CharacterId == characterId);

            if (character == null)
                return false;

            account.Characters.Remove(character);
            character.DeleteAndFlush();
            account.DeletedCharacters.Add(CreateAccountDeletedCharacter(account, world, characterId));
            account.SaveAndFlush();

            return true;
        }
    }
}