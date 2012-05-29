using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Marks
{
    [EffectHandler(EffectsEnum.Effect_Trap)]
    public class TrapSpawn : SpellEffectHandler
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public TrapSpawn(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var trapSpell = new Spell(Dice.DiceNum, (sbyte)Dice.DiceFace);

            if (trapSpell.Template == null || !trapSpell.ByLevel.ContainsKey(Dice.DiceFace))
            {
                logger.Error("Cannot find trap spell id = {0}, level = {1}. Casted Spell = {2}", Dice.DiceNum, Dice.DiceFace, Spell.Id);
                return false;
            }

            if (Fight.GetTriggers(TargetedCell).Length > 0)
            {
                if (Caster is CharacterFighter)
                    BasicHandler.SendTextInformationMessage(( Caster as CharacterFighter ).Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 229);

                return false;
            }

            // todo : find usage of Dice.Value
            var trap = EffectZone.ShapeType == SpellShapeEnum.Q ?
                new Trap((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, trapSpell, TargetedCell, GameActionMarkCellsTypeEnum.CELLS_CROSS, Effect.ZoneSize) :
                new Trap((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, trapSpell, TargetedCell, Effect.ZoneSize);

            Fight.AddTriger(trap);
            return true;
        }

        public override bool RequireSilentCast()
        {
            return true;
        }
    }
}