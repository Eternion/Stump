using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Summons
{
    [BrainIdentifier((int)MonsterIdEnum.TOFU_NOIR_4561)]
    [BrainIdentifier((int)MonsterIdEnum.TOFU_DODU_4562)]
    [BrainIdentifier((int)MonsterIdEnum.BOUFTOU_4563)]
    [BrainIdentifier((int)MonsterIdEnum.BOUFTOU_NOIR_4564)]
    [BrainIdentifier((int)MonsterIdEnum.DRAGONNET_ROUGE_4565)]
    [BrainIdentifier((int)MonsterIdEnum.DRAGONNET_NOIR_4566)]
    public class OsaSummonsBrain : Brain
    {
        public OsaSummonsBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        void OnGetAlive(FightActor fighter)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.LIEN_ANIMAL, 1), Fighter.Cell);
        }
    }
}
