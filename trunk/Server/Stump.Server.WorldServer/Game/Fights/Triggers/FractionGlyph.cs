#region License GNU GPL
// FractionGlyph.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Triggers
{
    public class FractionGlyph : MarkTrigger
    {
        public FractionGlyph(short id, FightActor caster, Spell castedSpell, EffectDice originEffect, Cell centerCell, params MarkShape[] shapes) : base(id, caster, castedSpell, originEffect, centerCell, shapes)
        {
        }

        public override GameActionMarkTypeEnum Type
        {
            get { return GameActionMarkTypeEnum.GLYPH; }
        }

        public override TriggerType TriggerType
        {
            get { return TriggerType.NEVER; }
        }

        public override void Trigger(FightActor trigger)
        {
            throw new System.NotImplementedException();
        }

        public override GameActionMark GetGameActionMark()
        {
            return new GameActionMark(Caster.Id, CastedSpell.Id, Id, (sbyte)Type,
                                      Shapes.Select(entry => entry.GetGameActionMarkedCell()));
        }

        public override GameActionMark GetHiddenGameActionMark()
        {
            return GetGameActionMark();
        }

        public override bool DoesSeeTrigger(FightActor fighter)
        {
            return true;
        }

        public int DispatchDamages(int damage, EffectSchoolEnum school, FightActor from, bool pvp = false, Spell spell = null, bool withBoost = true)
        {
            var actors = GetCells().Select(x => Fight.GetFirstFighter<FightActor>(x)).ToArray();

            if (spell != null)
                damage += from.GetSpellBoost(spell);

            if (withBoost)
                damage = from.CalculateDamage(damage, school);

            var percentResistance = GetAveragePercentResistance(actors, school, pvp);
            var fixResistance = GetAverageFixResistance(actors, school, pvp);
            damage = (int)( ( 1 - percentResistance / 100d ) * ( damage - fixResistance ) );

            var damagePerFighter = damage / actors.Length;

            foreach (var actor in actors)
            {
                if (from != actor)
                {
                    int reflected = actor.CalculateDamageReflection(damage);

                    if (reflected > 0)
                    {
                        from.InflictDirectDamage(reflected, actor);
                        actor.OnDamageReflected(from, reflected);
                    }
                }

                if (damage <= 0)
                    damage = 0;

                actor.InflictDirectDamage(damage, from);
            }

            return damagePerFighter;
        }

        private int GetAveragePercentResistance(FightActor[] actors, EffectSchoolEnum type, bool pvp)
        {
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.NeutralResistPercent].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpNeutralResistPercent].Total) : 0 ));
                case EffectSchoolEnum.Earth:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.EarthResistPercent].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpEarthResistPercent].Total) : 0 ));
                case EffectSchoolEnum.Air:
                    return (int) ( actors.Average(x => x.Stats[PlayerFields.AirResistPercent].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpAirResistPercent].Total) : 0 ));
                case EffectSchoolEnum.Water:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.WaterResistPercent].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpWaterResistPercent].Total) : 0 ));
                case EffectSchoolEnum.Fire:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.FireResistPercent].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpFireResistPercent].Total) : 0 ));
                default:
                    return 0;
            }
        }

        private int GetAverageFixResistance(FightActor[] actors, EffectSchoolEnum type, bool pvp)
        {
            switch (type)
            {
                case EffectSchoolEnum.Neutral:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.NeutralElementReduction].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpNeutralElementReduction].Total) : 0 ) + actors.Average(x => x.Stats[PlayerFields.PhysicalDamageReduction].Total));
                case EffectSchoolEnum.Earth:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.EarthElementReduction].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpEarthElementReduction].Total) : 0 ) + actors.Average(x => x.Stats[PlayerFields.PhysicalDamageReduction].Total));
                case EffectSchoolEnum.Air:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.AirElementReduction].Total) + (pvp ? actors.Average(x => x.Stats[PlayerFields.PvpAirElementReduction].Total) : 0) + actors.Average(x => x.Stats[PlayerFields.MagicDamageReduction].Total));
                case EffectSchoolEnum.Water:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.WaterElementReduction].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpWaterElementReduction].Total) : 0 ) + actors.Average(x => x.Stats[PlayerFields.MagicDamageReduction].Total));
                case EffectSchoolEnum.Fire:
                    return (int) (actors.Average(x => x.Stats[PlayerFields.FireElementReduction].Total) + ( pvp ? actors.Average(x => x.Stats[PlayerFields.PvpFireElementReduction].Total) : 0 ) + actors.Average(x => x.Stats[PlayerFields.MagicDamageReduction].Total));
                default:
                    return 0;
            }
        }
    }
}