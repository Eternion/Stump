using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Social
{
    public class Ignored
    {
        public Ignored(AccountRelation relation, WorldAccount account, bool session)
        {
            Relation = relation;
            Session = session;
            Account = account;
        }

        public Ignored(AccountRelation relation, WorldAccount account, bool session, Character character)
        {
            Relation = relation;
            Session = session;
            Account = account;
            Character = character;
        }

        public bool Session
        {
            get;
            private set;
        }

        public WorldAccount Account
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }

        public AccountRelation Relation
        {
            get;
            private set;
        }

        public void SetOnline(Character character)
        {
            if (character.Client.WorldAccount.Id != Account.Id)
                return;

            Character = character;
        }

        public void SetOffline()
        {
            Character = null;
        }

        public bool IsOnline()
        {
            return Character != null;
        }
            
        public IgnoredInformations GetIgnoredInformations()
        {
            if (IsOnline())
            {
                return new IgnoredOnlineInformations(Account.Id,
                    Account.Nickname,
                    Character.Name,
                    (sbyte)Character.Breed.Id,
                    Character.Sex == SexTypeEnum.SEX_FEMALE);
            }

            return new IgnoredInformations(
                Account.Id,
                Account.Nickname);
        } 
    }
}