using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Spells;
using System;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class FractionBuff : Buff
    {
        public FractionBuff(int id, FightActor target, FightActor caster, SpellEffectHandler effect, Spell spell, bool critical, FightDispellableEnum dispelable, FightActor[] fighters)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Fighters = fighters;
        }

        FightActor[] m_fighters;

        public FightActor[] Fighters
        {
            get { return m_fighters.Where(x => x.IsAlive()).ToArray(); }
            set { m_fighters = value; }
        }

        public IFight Fight => Caster.Fight;

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var turnDuration = Delay == 0 ? Duration : Delay;

            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, turnDuration,
                (sbyte)Dispellable,
                (short)Spell.Id, EffectFix?.ClientEffectId ?? Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }

        public int DispatchDamages(Damage damage)
        {
            damage.GenerateDamages();

            var percentResistance = GetAveragePercentResistance(Fighters, damage.School, Fight.IsPvP);
            var fixResistance = GetAverageFixResistance(Fighters, damage.School, Fight.IsPvP);
            var armor = GetAverageArmor(Fighters, damage.School);

            damage.Amount = (int)((1 - percentResistance / 100d) * (damage.Amount - armor - fixResistance));
            damage.Amount = (damage.Amount / Fighters.Length);
            damage.IgnoreDamageReduction = true;

            foreach (var actor in Fighters)
            {
                var damagePerFighter = new FractionDamage(damage.Amount)
                {
                    Source = damage.Source,
                    School = damage.School,
                    Buff = damage.Buff,
                    IgnoreDamageReduction = true,
                    IgnoreDamageBoost = true,
                    EffectGenerationType = damage.EffectGenerationType,
                    IsCritical = damage.IsCritical
                };

                actor.InflictDamage(damagePerFighter);
            }

            return damage.Amount;
        }

        static int GetAverageArmor(FightActor[] actors, EffectSchoolEnum type)
        {
            int specificArmor;
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    specificArmor = (int) actors.Average(x => x.Stats[PlayerFields.NeutralDamageArmor].TotalSafe);
                    break;
                case EffectSchoolEnum.Earth:
                    specificArmor = (int)actors.Average(x => x.Stats[PlayerFields.EarthDamageArmor].TotalSafe);
                    break;
                case EffectSchoolEnum.Air:
                    specificArmor = (int)actors.Average(x => x.Stats[PlayerFields.AirDamageArmor].TotalSafe);
                    break;
                case EffectSchoolEnum.Water:
                    specificArmor = (int)actors.Average(x => x.Stats[PlayerFields.WaterDamageArmor].TotalSafe);
                    break;
                case EffectSchoolEnum.Fire:
                    specificArmor = (int)actors.Average(x => x.Stats[PlayerFields.FireDamageArmor].TotalSafe);
                    break;
                default:
                    return 0;
            }

            return specificArmor + (int)actors.Average(x => x.Stats[PlayerFields.GlobalDamageReduction].Total);
        }
        
        static int GetAveragePercentResistance(FightActor[] actors, EffectSchoolEnum type, bool pvp)
        {
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.NeutralResistPercent].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpNeutralResistPercent].Total) : 0));
                case EffectSchoolEnum.Earth:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.EarthResistPercent].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpEarthResistPercent].Total) : 0));
                case EffectSchoolEnum.Air:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.AirResistPercent].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpAirResistPercent].Total) : 0));
                case EffectSchoolEnum.Water:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.WaterResistPercent].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpWaterResistPercent].Total) : 0));
                case EffectSchoolEnum.Fire:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.FireResistPercent].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpFireResistPercent].Total) : 0));
                default:
                    return 0;
            }
        }

        static int GetAverageFixResistance(FightActor[] actors, EffectSchoolEnum type, bool pvp)
        {
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.NeutralElementReduction].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpNeutralElementReduction].Total) : 0) +
                             actors.Average(x => x.Stats[PlayerFields.PhysicalDamageReduction].Total));
                case EffectSchoolEnum.Earth:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.EarthElementReduction].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpEarthElementReduction].Total) : 0) +
                             actors.Average(x => x.Stats[PlayerFields.PhysicalDamageReduction].Total));
                case EffectSchoolEnum.Air:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.AirElementReduction].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpAirElementReduction].Total) : 0) +
                             actors.Average(x => x.Stats[PlayerFields.MagicDamageReduction].Total));
                case EffectSchoolEnum.Water:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.WaterElementReduction].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpWaterElementReduction].Total) : 0) +
                             actors.Average(x => x.Stats[PlayerFields.MagicDamageReduction].Total));
                case EffectSchoolEnum.Fire:
                    return
                        (int)
                            (actors.Average(x => x.Stats[PlayerFields.FireElementReduction].Total) +
                             (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpFireElementReduction].Total) : 0) +
                             actors.Average(x => x.Stats[PlayerFields.MagicDamageReduction].Total));
                default:
                    return 0;
            }
        }
    }

    public class FractionDamage : Damage
    {
        public FractionDamage(int amount) : base(amount)
        {
        }
    }
}
