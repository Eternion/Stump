using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Social
{
    public class Friend
    {
        public Friend(AccountRelation relation, WorldAccount account)
        {
            Relation = relation;
            Account = account;
        }

        public Friend(AccountRelation relation, WorldAccount account, Character character)
        {
            Relation = relation;
            Account = account;
            Character = character;
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

        public FriendInformations GetFriendInformations(Character asker)
        {
            if (IsOnline())
            {
                return new FriendOnlineInformations(Account.Id,
                    Account.Nickname,
                    (sbyte)( Character.IsFighting() ? PlayerStateEnum.GAME_TYPE_FIGHT : PlayerStateEnum.GAME_TYPE_ROLEPLAY ),
                    (short)Account.LastConnectionTimeStamp,
                    0, // todo achievement
                    Character.Id,
                    Character.Name,
                    Character.FriendsBook.IsFriend(asker.Account.Id) ? Character.Level : (byte)0,
                    Character.FriendsBook.IsFriend(asker.Account.Id) ? (sbyte)Character.AlignmentSide : (sbyte)AlignmentSideEnum.ALIGNMENT_UNKNOWN,
                    (sbyte)Character.Breed.Id,
                    Character.Sex == SexTypeEnum.SEX_FEMALE,
                    Character.GuildMember == null ? new GuildInformations(0, "", 0, new GuildEmblem(0,0,0,0)) : Character.GuildMember.Guild.GetGuildInformations(),
                    (short)Character.SmileyMoodId,
                    Character.Status);
            }

            return new FriendInformations(
                Account.Id,
                Account.Nickname,
                (sbyte) PlayerStateEnum.NOT_CONNECTED,
                (short)Account.LastConnectionTimeStamp,
                0); // todo achievement
        }
    }
}