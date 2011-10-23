using System;
using System.Drawing;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Effects.Instances;
using Stump.Server.WorldServer.Worlds.Fights.Triggers;
using Stump.Server.WorldServer.Worlds.Spells;

namespace Stump.Server.WorldServer.Worlds.Effects.Handlers.Spells.Marks
{
    [EffectHandler(EffectsEnum.Effect_Trap)]
    public class TrapSpawn : SpellEffectHandler
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public TrapSpawn(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override void Apply()
        {
            var trapSpell = new Spell(Dice.DiceNum, (sbyte)Dice.DiceFace);

            if (trapSpell.Template == null || !trapSpell.ByLevel.ContainsKey(Dice.DiceFace))
            {
                logger.Error("Cannot find trap spell id = {0}, level = {1}. Casted Spell = {2}", Dice.DiceNum, Dice.DiceFace, Spell.Id);
                return;
            }

            // todo : find usage of Dice.Value
            var trap = EffectZone.ShapeType == SpellShapeEnum.Q ?
                new Trap((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, trapSpell, TargetedCell, GameActionMarkCellsTypeEnum.CELLS_CROSS, (sbyte)Effect.ZoneSize) :
                new Trap((short)Fight.PopNextTriggerId(), Caster, Spell, Dice, trapSpell, TargetedCell, (sbyte)Effect.ZoneSize);

            Fight.AddTriger(trap);
        }
    }
}