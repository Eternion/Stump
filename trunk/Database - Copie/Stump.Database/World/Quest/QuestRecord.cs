
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;

namespace Stump.Database.WorldServer.Quest
{
    [ActiveRecord("characters_quests")]
    public class QuestRecord : WorldBaseRecord<QuestRecord>
    {
        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id { get; set; }

        [BelongsTo("CharacterId", NotNull = true)]
        public CharacterRecord Character { get; set; }

        [Property("QuestId", NotNull = true)]
        public uint QuestId { get; set; }

        [Property("Step", NotNull = true, Default = "0")]
        public uint Step { get; set; }
    }
}