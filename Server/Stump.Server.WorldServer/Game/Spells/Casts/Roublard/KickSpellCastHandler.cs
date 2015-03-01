using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
{
    [SpellCastHandler(SpellIdEnum.BOTTE)]
    public class KickSpellCastHandler : DefaultSpellCastHandler
    {
        public KickSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var handler in Handlers.OfType<Push>())
            {
                handler.DamagesDisabled = true;
                var fighter = Fight.GetFirstFighter<SummonedBomb>(TargetedCell);
                if (fighter != null && fighter.IsFriendlyWith(Caster))
                    handler.SubRangeForActor = fighter;
            }
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            foreach (var handler in Handlers.OrderByDescending(entry => entry.Dice.DiceNum))
            {
                handler.Apply();
            }
        }
    }
}