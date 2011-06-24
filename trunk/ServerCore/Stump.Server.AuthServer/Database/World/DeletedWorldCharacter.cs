using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Server.AuthServer.Database.Account;

namespace Stump.Server.AuthServer.Database.World
{
    [Serializable]
    [ActiveRecord("worlds_characters_deleted")]
    public sealed class DeletedWorldCharacter : AuthBaseRecord<DeletedWorldCharacter>
    {

        public DeletedWorldCharacter()
        {
            DeletionDate = DateTime.Now;
        }

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id
        {
            get;
            set;
        }

        [BelongsTo(Column = "AccountId", NotNull = true)]
        public Account.Account Account
        {
            get;
            set;
        }

        [BelongsTo(Column = "WorldId", NotNull = true)]
        public WorldServer World
        {
            get;
            set;
        }

        [KeyProperty(Column = "CharacterId", NotNull = true)]
        public uint CharacterId
        {
            get;
            set;
        }

        [KeyProperty(Column = "DeletionDate", NotNull = true)]
        public DateTime DeletionDate
        {
            get;
            set;
        }


        public static DeletedWorldCharacter FindCharacterById(long id)
        {
            return FindByPrimaryKey(id);
        }

        public static DeletedWorldCharacter[] FindCharactersByAccount(Account.Account account)
        {
            return FindAll((Restrictions.Eq("Account", account)));
        }

        public static DeletedWorldCharacter[] FindCharactersByServer(WorldServer world)
        {
            return FindAll((Restrictions.Eq("World", world)));
        }

        public static DeletedWorldCharacter FindCharacterByServerAndCharacterId(WorldServer world, uint characterId)
        {
            return FindOne(Restrictions.And(Restrictions.Eq("World", world), Restrictions.Eq("CharacterId", characterId)));
        }

        public static DeletedWorldCharacter[] FindCharactersByAccountAndServer(Account.Account account, WorldServer world)
        {
            return FindAll(Restrictions.And(Restrictions.Eq("Account", account), Restrictions.Eq("World", world)));
        }

    }
}