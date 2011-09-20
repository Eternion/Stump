using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs
{
    [ActiveRecord("npcs_actions", DiscriminatorColumn = "RecognizerType", DiscriminatorType = "String", DiscriminatorValue = "Base")]
    public abstract class NpcAction : WorldBaseRecord<NpcAction>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("NpcId")]
        public NpcTemplate Npc
        {
            get;
            set;
        }

        public abstract NpcActionTypeEnum ActionType
        {
            get;
        }

        public abstract void Execute(Npc npc, Character character);
    }
}