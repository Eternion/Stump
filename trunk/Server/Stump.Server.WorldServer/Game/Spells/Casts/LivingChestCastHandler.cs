using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [SpellCastHandler(SpellIdEnum.LivingChest)]
    public class LivingChestCastHandler : DefaultSpellCastHandler
    {
        public LivingChestCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            if (Handlers.Length != 2)
                return;

            var killEffect = Handlers[0] as Kill;
            var summonEffect = Handlers[1] as Summon;

            if (killEffect == null || summonEffect == null)
                return;

            var chests = Fight.GetAllFighters<SummonedMonster>(entry =>
                        entry.Team == Caster.Team &&
                        entry.Monster.MonsterId == summonEffect.Dice.DiceNum);

            killEffect.SetAffectedActors(chests);
            killEffect.Apply();

            summonEffect.Apply();
        }
    }
}