using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Spells.Casts;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Marks
{
    [EffectHandler(EffectsEnum.Effect_Rune)]
    public class RuneSpawn : SpellEffectHandler
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public RuneSpawn(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var runeSpell = new Spell(Dice.DiceNum, (byte)Dice.DiceFace);

            if (runeSpell.Template == null || !runeSpell.ByLevel.ContainsKey(Dice.DiceFace))
            {
                logger.Error("Cannot find rune spell id = {0}, level = {1}. Casted Spell = {2}", Dice.DiceNum, Dice.DiceFace, Spell.Id);
                return false;
            }

            foreach (var trigger in Fight.GetTriggersByCell(TargetedCell).OfType<Rune>().Where(x => x.Caster.IsFriendlyWith(Caster)))
            {
                trigger.Remove();
            }

            var rune = new Rune((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, runeSpell, TargetedCell, EffectZone.ShapeType, (byte)Effect.ZoneMinSize, (byte)Effect.ZoneSize);

            Fight.AddTriger(rune);

            return true;
        }
    }
}