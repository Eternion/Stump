
using System;
using System.Collections.Generic;
using Stump.Core.Pool.Task;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Helpers
{
    public static class AccountManager
    {
        public static List<CharacterRecord> GetCharactersOnAccount(WorldClient client)
        {
            var characters = new List<CharacterRecord>((int)CharacterHandler.MaxCharacterSlot);
            var ids = client.Account.GetWorldCharactersId(WorldServer.ServerInformation.Id);

            foreach (var id in ids)
            {
                /* don't exists */
                if (!CharacterRecord.Exists(id))
                {
                    TaskPool.Instance.EnqueueTask(() =>
                                                  IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(
                                                      WorldServer.ServerInformation, client.Account.Id, id));
                }
                else
                {
                    characters.Add(CharacterRecord.FindById((int)id));
                }
            }
            return characters;
        }

        public static void AddCharacterOnAccount(CharacterRecord character, WorldClient client)
        {
            if (client.Characters == null)
                client.Characters = new List<CharacterRecord>((int)CharacterHandler.MaxCharacterSlot);

            client.Characters.Insert(0, character);

            TaskPool.Instance.EnqueueTask(() => IpcAccessor.Instance.ProxyObject.AddAccountCharacter(
                WorldServer.ServerInformation, client.Account.Id, (uint)character.Id));
        }

        public static void RemoveCharacterFromAccount(CharacterRecord character, WorldClient client)
        {
            client.Characters.Remove(character);

            TaskPool.Instance.EnqueueTask(() => IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(
                WorldServer.ServerInformation, client.Account.Id, (uint)character.Id));
        }

        public static int GetNumberOfDayDeletedCharacter(uint accountId)
        {
            return IpcAccessor.Instance.ProxyObject.GetDeletedCharactersNumber(accountId);
        }

        public static AccountRecord GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByTicket(WorldServer.ServerInformation, ticket);
        }

        public static AccountRecord GetAccountByNickname(string nickName)
        {
            if (!IpcAccessor.Instance.Connected ||  IpcAccessor.Instance.ProxyObject == null)
                throw new Exception("Cannot acces to AuthServer, check that the server is running");

            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByNickname(WorldServer.ServerInformation, nickName);
        }

        public static RoleEnum GetRoleByNickName(string nickName)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null ? RoleEnum.None : acc.Role;
        }

        public static RoleEnum GetRoleWithCheckPassword(string nickName, string accPass)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null || acc.Password != accPass ? RoleEnum.None : acc.Role;
        }
    }
}
