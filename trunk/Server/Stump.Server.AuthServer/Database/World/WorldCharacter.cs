using System;
using Castle.ActiveRecord;
using Stump.Server.AuthServer.Database.Account;

namespace Stump.Server.AuthServer.Database.World
{
    [Serializable]
    [ActiveRecord("worlds_characters")]
    public class WorldCharacter : AuthBaseRecord<WorldCharacter>
    {

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

        [Property(Column = "CharacterId", NotNull = true)]
        public uint CharacterId
        {
            get;
            set;
        }

    }
}