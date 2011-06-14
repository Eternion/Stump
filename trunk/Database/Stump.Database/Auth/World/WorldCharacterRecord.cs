
using System;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.AuthServer.World
{
    [Serializable]
    [ActiveRecord("worlds_characters")]
    public class WorldCharacterRecord : AuthBaseRecord<WorldCharacterRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id
        {
            get;
            set;
        }

        [BelongsTo(Column = "AccountId", NotNull = true)]
        public AccountRecord Account
        {
            get;
            set;
        }

        [BelongsTo(Column = "WorldId", NotNull = true)]
        public WorldRecord World
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

    }
}