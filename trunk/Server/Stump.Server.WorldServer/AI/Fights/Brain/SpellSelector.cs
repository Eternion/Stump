using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class SpellSelector
    {
        public SpellSelector(AIFighter fighter)
        {
            Fighter = fighter;
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public Spell GetBestSpell()
        {
            Spell bestSpell = null;
            double bestSpellDamages = 0;
            foreach (Spell spell in Fighter.Spells.Values)
            {
                double spellDamage = GetSpellDamages(spell);
                if (spellDamage > bestSpellDamages)
                {
                    bestSpell = spell;
                    bestSpellDamages = spellDamage;
                }
            }

            return bestSpell;
        }

        private static double GetSpellDamages(Spell spell)
        {
            double sum = 0;
            foreach (EffectDice effect in spell.CurrentSpellLevel.Effects)
            {
                if (effect.EffectId >= EffectsEnum.Effect_DamageWater &&
                    effect.EffectId <= EffectsEnum.Effect_DamageNeutral)
                {
                    short max = effect.DiceNum >= effect.DiceFace ? effect.DiceNum : effect.DiceFace;
                    short min = effect.DiceNum <= effect.DiceFace ? effect.DiceNum : effect.DiceFace;

                    sum += ( max + min ) / 2d;
                }
            }

            return sum;
        }
    }
}