using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class FractionBuff : Buff
    {
        public FractionBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, FightActor[] fighters)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Fighters = fighters;
        }

        public FractionBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
        }

        public FightActor[] Fighters
        {
            get;
            set;
        }

        public IFight Fight
        {
            get { return Caster.Fight; }
        }

        public override void Apply()
        {
            
        }

        public override void Dispell()
        {

        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? 0 : 1), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }

        public int DispatchDamages(Damage damage)
        {
            damage.GenerateDamages();

            if (Fighters.Count() < 2)
                return damage.Amount;

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

        private static int GetAverageArmor(FightActor[] actors, EffectSchoolEnum type)
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
        
        private static int GetAveragePercentResistance(FightActor[] actors, EffectSchoolEnum type, bool pvp)
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

        private static int GetAverageFixResistance(FightActor[] actors, EffectSchoolEnum type, bool pvp)
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
