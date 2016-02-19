using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Handlers.Basic;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Marks
{
    [EffectHandler(EffectsEnum.Effect_Trap)]
    public class TrapSpawn : SpellEffectHandler
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public TrapSpawn(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var trapSpell = new Spell(Dice.DiceNum, (byte)Dice.DiceFace);

            if (trapSpell.Template == null || !trapSpell.ByLevel.ContainsKey(Dice.DiceFace))
            {
                logger.Error("Cannot find trap spell id = {0}, level = {1}. Casted Spell = {2}", Dice.DiceNum, Dice.DiceFace, Spell.Id);
                return false;
            }

            var trap = new Trap((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, trapSpell, TargetedCell, EffectZone.ShapeType, (byte) Effect.ZoneSize);

            if (Spell.Id == (int) SpellIdEnum.PIÈGE_RÉPULSIF)
                trap.Priority = -1;

            Fight.AddTriger(trap);
            return true;
        }

        public override bool CanApply()
        {
            if (Fight.GetTriggers(TargetedCell).Length <= 0)
                return base.CanApply();

            if (Caster is CharacterFighter)
                BasicHandler.SendTextInformationMessage(((CharacterFighter)Caster).Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 229);

            return false;
        }
    }
}