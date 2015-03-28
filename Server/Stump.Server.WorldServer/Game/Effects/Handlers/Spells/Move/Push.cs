using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Handlers.Actions;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_PushBack)]
    [EffectHandler(EffectsEnum.Effect_PushBack_1103)]
    public class Push : SpellEffectHandler
    {
        public Push(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public bool DamagesDisabled
        {
            get;
            set;
        }

        public FightActor SubRangeForActor
        {
            get;
            set;
        }

        public override bool Apply()
        {
            var integerEffect = GenerateEffect();

            if (integerEffect == null)
                return false;

            foreach (var actor in GetAffectedActors().OrderByDescending(entry => entry.Position.Point.ManhattanDistanceTo(TargetedPoint)))
            {
                if (actor.HasState((int)SpellStatesEnum.Unmovable) || actor.HasState((int)SpellStatesEnum.Rooted))
                    continue;

                var referenceCell = TargetedCell.Id == actor.Cell.Id ? CastPoint : TargetedPoint;

                if (referenceCell.CellId == actor.Position.Cell.Id)
                    continue;

                var pushDirection = referenceCell.OrientationTo(actor.Position.Point);
                var startCell = actor.Position.Point;
                var lastCell = startCell;
                var range = SubRangeForActor == actor ? (integerEffect.Value - 1) : integerEffect.Value;
                var takeDamage = false;

                for (var i = 0; i < range; i++)
                {
                    var nextCell = lastCell.GetNearestCellInDirection(pushDirection);

                    if (nextCell == null || !Fight.IsCellFree(Map.Cells[nextCell.CellId]))
                    {
                        var pushbackDamages = Formulas.FightFormulas.CalculatePushBackDamages(Caster, actor, (range - i));

                        if (!DamagesDisabled)
                        {
                            var damage = new Fights.Damage(pushbackDamages)
                            {
                                Source = actor,
                                School = EffectSchoolEnum.Pushback,
                                IgnoreDamageBoost = true,
                                IgnoreDamageReduction = false
                            };

                            takeDamage = true;
                            actor.InflictDamage(damage);

                            if (nextCell != null)
                            {
                                var fighter = Fight.GetOneFighter(Map.Cells[nextCell.CellId]);
                                if (fighter != null)
                                {
                                    pushbackDamages = pushbackDamages / 2 - fighter.Stats[PlayerFields.PushDamageReduction];
                                    damage = new Fights.Damage(pushbackDamages)
                                    {
                                        Source = fighter,
                                        School = EffectSchoolEnum.Pushback,
                                        IgnoreDamageBoost = true,
                                        IgnoreDamageReduction = false
                                    };

                                    fighter.InflictDamage(damage);
                                    fighter.TriggerBuffs(BuffTriggerType.DAMAGES_PUSHBACK);

                                    fighter.OnActorMoved(actor, true);
                                }
                            }
                        }

                        break;
                    }

                    if (Fight.ShouldTriggerOnMove(Fight.Map.Cells[nextCell.CellId], actor))
                    {
                        lastCell = nextCell;
                        break;
                    }

                    lastCell = nextCell;
                }

                var endCell = lastCell;

                if (actor.IsCarrying())
                    actor.ThrowActor(Map.Cells[startCell.CellId], true);

                foreach (var fighter in Fight.GetAllFighters<CharacterFighter>().Where(actor.IsVisibleFor))
                    ActionsHandler.SendGameActionFightSlideMessage(fighter.Character.Client, Caster, actor, startCell.CellId, endCell.CellId);

                actor.Position.Cell = Map.Cells[endCell.CellId];
                actor.OnActorMoved(Caster, takeDamage);
                if (takeDamage)
                    actor.TriggerBuffs(BuffTriggerType.DAMAGES_PUSHBACK);
            }

            return true;
        }
    }
}