using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.Characters;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Friends;

namespace Stump.Server.WorldServer.Game.Social
{
    public class FriendsBook : IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable(true)]
        public static int MaxFriendsNumber = 30;

        private List<Friend> m_friends = new List<Friend>();
        private List<Ignored> m_ignoreds = new List<Ignored>();
        private List<AccountRelation> m_relations;
        private Stack<AccountRelation> m_relationsToRemove = new Stack<AccountRelation>();

        public FriendsBook(Character owner)
        {
            Owner = owner;
        }

        public Character Owner
        {
            get;
            set;
        }

        public ReadOnlyCollection<Friend> Friends
        {
            get { return m_friends.AsReadOnly(); }
        }

        public ReadOnlyCollection<Ignored> Ignoreds
        {
            get { return m_ignoreds.AsReadOnly(); }
        }

        public bool WarnOnConnection
        {
            get { return Owner.Record.WarnOnConnection; }
            set
            {
                Owner.Record.WarnOnConnection = value;
                FriendHandler.SendFriendWarnOnConnectionStateMessage(Owner.Client, value);
            }
        }

        public bool WarnOnLevel
        {
            get
            {
                return Owner.Record.WarnOnLevel;
            }
            set
            {
                Owner.Record.WarnOnLevel = value;
                FriendHandler.SendFriendWarnOnLevelGainStateMessage(Owner.Client, value);
            }
        }

        public ListAddFailureEnum? CanAddFriend(WorldAccount friendAccount)
        {
            if (friendAccount.Id == Owner.Client.WorldAccount.Id)
                return ListAddFailureEnum.LIST_ADD_FAILURE_EGOCENTRIC;

            if (m_friends.Any(entry => entry.Account.Id == friendAccount.Id))
                return ListAddFailureEnum.LIST_ADD_FAILURE_IS_DOUBLE;

            if (m_friends.Count >= MaxFriendsNumber)
                return ListAddFailureEnum.LIST_ADD_FAILURE_OVER_QUOTA;

            return null;
        }


        public bool AddFriend(WorldAccount friendAccount)
        {
            var result = CanAddFriend(friendAccount);

            if (result != null)
            {
                FriendHandler.SendFriendAddFailureMessage(Owner.Client, result.Value);
                return false;
            }

            var relation = new AccountRelation()
            {
                AccountId = Owner.Client.Account.Id,
                TargetId = friendAccount.Id,
                Type = AccountRelationType.Friend
            };
            
            m_relations.Add(relation);

            if (friendAccount.ConnectedCharacter.HasValue)
            {
                var character = World.Instance.GetCharacter(friendAccount.ConnectedCharacter.Value);
                var friend = new Friend(relation, friendAccount, character);
                m_friends.Add(friend);

                OnFriendOnline(friend);
            }
            else
                m_friends.Add(new Friend(relation, friendAccount));


            FriendHandler.SendFriendsListMessage(Owner.Client, Friends);

            return true;
        }

        public bool RemoveFriend(Friend friend)
        {
            if (friend.IsOnline())
                OnCharacterLogout(friend.Character); // unregister the events

            if (m_friends.Remove(friend))
            {
                m_relationsToRemove.Push(friend.Relation);
                FriendHandler.SendFriendDeleteResultMessage(Owner.Client, true, friend.Account.Nickname);

                return true;
            }
            else
            {
                FriendHandler.SendFriendDeleteResultMessage(Owner.Client, false, friend.Account.Nickname);

                return false;
            }
        }

        public ListAddFailureEnum? CanAddIgnored(WorldAccount ignoredAccount)
        {
            if (ignoredAccount.Id == Owner.Client.WorldAccount.Id)
                return ListAddFailureEnum.LIST_ADD_FAILURE_EGOCENTRIC;

            if (m_ignoreds.Any(entry => entry.Account.Id == ignoredAccount.Id))
                return ListAddFailureEnum.LIST_ADD_FAILURE_IS_DOUBLE;

            if (m_ignoreds.Count >= MaxFriendsNumber)
                return ListAddFailureEnum.LIST_ADD_FAILURE_OVER_QUOTA;

            return null;
        }

        public bool AddIgnored(WorldAccount ignoredAccount, bool session = false)
        {
            var result = CanAddIgnored(ignoredAccount);

            if (result != null)
            {
                FriendHandler.SendIgnoredAddFailureMessage(Owner.Client, result.Value);
                return false;
            }

            var relation = new AccountRelation()
                {
                    AccountId = Owner.Client.Account.Id,
                    TargetId = ignoredAccount.Id,
                    Type = AccountRelationType.Ignored
                };

            if (!session)
                m_relations.Add(relation);

            if (ignoredAccount.ConnectedCharacter.HasValue)
            {
                var character = World.Instance.GetCharacter(ignoredAccount.ConnectedCharacter.Value);

                m_ignoreds.Add(new Ignored(relation, ignoredAccount, session, character));
            }
            else
                m_ignoreds.Add(new Ignored(relation, ignoredAccount, session));


            FriendHandler.SendIgnoredListMessage(Owner.Client, Ignoreds);

            return true;
        }

        public bool RemoveIgnored(Ignored ignored)
        {
            if (m_ignoreds.Remove(ignored))
            {
                m_relationsToRemove.Push(ignored.Relation);
                FriendHandler.SendIgnoredDeleteResultMessage(Owner.Client, true, ignored.Session, ignored.Account.Nickname);
                
                return true;
            }
            else
            {
                FriendHandler.SendIgnoredDeleteResultMessage(Owner.Client, false, ignored.Session, ignored.Account.Nickname);

                return false;
            }
        }

        private void OnCharacterLogIn(Character character)
        {
            if (m_friends.Any(entry => entry.Account.Id == character.Client.WorldAccount.Id))
            {
                foreach (var friend in m_friends.Where(entry => entry.Account.Id == character.Client.WorldAccount.Id))
                {
                    friend.SetOnline(character);
                    OnFriendOnline(friend);

                    if (WarnOnConnection)
                    {
                        BasicHandler.SendTextInformationMessage(Owner.Client,
                            TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 143, character.Client.WorldAccount.Nickname, character.Name);
                    }
                }
            }

            if (m_ignoreds.Any(entry => entry.Account.Id == character.Client.WorldAccount.Id))
            {
                foreach (var ignored in m_ignoreds.Where(entry => entry.Account.Id == character.Client.WorldAccount.Id))
                {
                    ignored.SetOnline(character);
                }
            }
        }

        private void OnFriendOnline(Friend friend)
        {
            friend.Character.LoggedOut += OnCharacterLogout;
            friend.Character.LevelChanged += OnLevelChanged;
            friend.Character.ContextChanged += OnContextChanged;
        }

        private void OnContextChanged(Character character, bool infight)
        {
            var friend = TryGetFriend(character);

            if (friend == null)
            {
                logger.Error("Sad, friend bound with character {0} is not found :(", character);
                return;
            }

            FriendHandler.SendFriendUpdateMessage(Owner.Client, friend);
        }

        private void OnLevelChanged(Character character, byte currentlevel, int difference)
        {
            var friend = TryGetFriend(character);

            if (friend == null)
            {
                logger.Error("Sad, friend bound with character {0} is not found :(", character);
                return;
            }

            FriendHandler.SendFriendUpdateMessage(Owner.Client, friend);

            if (WarnOnLevel)
            {
                if (character.Map != Owner.Map)
                {
                    CharacterHandler.SendCharacterLevelUpInformationMessage(Owner.Client, character, character.Level);
                }
            }
        }

        private void OnCharacterLogout(Character character)
        {
            var friend = TryGetFriend(character);

            if (friend == null)
            {
                logger.Error("Sad, friend bound with character {0} is not found :(", character);
            }
            else
            {
                friend.SetOffline();
            }

            character.LoggedOut -= OnCharacterLogout;
            character.LevelChanged -= OnLevelChanged;
            character.ContextChanged -= OnContextChanged;
        }

        public void Load()
        {
            m_relations = WorldServer.Instance.DBAccessor.Database.Fetch<AccountRelation>(string.Format(AccountRelationRelator.FetchByAccount, Owner.Account.Id));
            foreach (var relation in m_relations)
            {
                var account = World.Instance.GetConnectedAccount(relation.TargetId);

                if (account == null)
                    account = AccountManager.Instance.FindById(relation.TargetId);

                // doesn't exist anymore, so we delete it
                if (account == null)
                {
                    WorldServer.Instance.DBAccessor.Database.Delete(relation);
                    continue;
                }

                if (relation.Type == AccountRelationType.Friend)
                {
                    if (account.ConnectedCharacter.HasValue)
                    {
                        var character = World.Instance.GetCharacter(account.ConnectedCharacter.Value);

                        m_friends.Add(new Friend(relation, account, character));
                    }
                    else
                        m_friends.Add(new Friend(relation, account));
                }
                else if (relation.Type == AccountRelationType.Ignored)
                {
                    if (account.ConnectedCharacter.HasValue)
                    {
                        var character = World.Instance.GetCharacter(account.ConnectedCharacter.Value);

                        m_ignoreds.Add(new Ignored(relation, account, false, character));
                    }
                    else
                        m_ignoreds.Add(new Ignored(relation, account, false));
                }
            }

            World.Instance.CharacterJoined += OnCharacterLogIn;
        }

        public void Save()
        {
            var database = WorldServer.Instance.DBAccessor.Database;
            foreach (var relation in m_relations)
            {
                database.Save(relation);
            }
            while (m_relationsToRemove.Count != 0)
            {
                database.Delete(m_relationsToRemove.Pop());
            }
        }

        public Friend TryGetFriend(Character character)
        {
            return m_friends.FirstOrDefault(entry => entry.Character != null && entry.Character.Id == character.Id);
        }

        public void Dispose()
        {
            World.Instance.CharacterJoined -= OnCharacterLogIn;
        }
    }
}