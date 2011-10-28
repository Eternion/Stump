using Castle.ActiveRecord;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs_replies", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class NpcReply : WorldBaseRecord<NpcReply>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property]
        public int ReplyId
        {
            get;
            set;
        }

        [BelongsTo("MessageId")]
        public NpcMessage Message
        {
            get;
            set;
        }

        public abstract void Execute(Npc npc, Character character);
    }
}