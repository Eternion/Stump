using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.AI.Fights.Brain;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Spells;

namespace GiantKralovePlugin
{
    [BrainIdentifier(423)] //MONSTER ID OF GIANT KRALOVE
    public class GiantKraloveBrain : Brain
    {
        private EffectSchoolEnum m_lastEffectSchool = EffectSchoolEnum.Unknown;
       
        public GiantKraloveBrain(AIFighter fighter) : base(fighter)
        {
            Fighter.DamageInflicted += OnDamageInflicted;
        }


        /*
         * 659 Kracheau
         * 1104 Kracheau ralentissant
         * 1105 Kracheau immobilisant
         * 1279 Tourbe écrasante
         * 1103 Kraken
         * 1107 Invocation de Tentacule Primaire (Monster id 424)
         * 1108 Invocation de Tentacule Secondaire
         * 1109 Invocation de Tentacule Tertiaire
         * 1110 Invocation de Tentacule Quaternaire
         * 
         */

        public override void Play()
        {
            if (m_lastEffectSchool != EffectSchoolEnum.Unknown)
            {
                Spell spellSummon = null;
                switch (m_lastEffectSchool)
                {
                    case EffectSchoolEnum.Air:
                        spellSummon = Fighter.GetSpell(1107);
                        break;
                    case EffectSchoolEnum.Earth:
                        spellSummon = Fighter.GetSpell(1108);
                        break;
                    case EffectSchoolEnum.Fire:
                        spellSummon = Fighter.GetSpell(1109);
                        break;
                    case EffectSchoolEnum.Water:
                        spellSummon = Fighter.GetSpell(1110);
                        break;
                }

                if (spellSummon != null)
                {
                    Cell cell;
                    Fighter.Fight.FindRandomFreeCell(this.Fighter, out cell, false); // TODO
                    Fighter.CastSpell(spellSummon, cell);
                }
            }
        }

        private bool ContainsSummonId(int id)
        {
            return Fighter.GetSummons().OfType<SummonedMonster>().Any(monster => monster.Monster.MonsterId == id);
        }

        private void OnDamageInflicted(FightActor fightActor, int damage, EffectSchoolEnum school, FightActor from)
        {
            m_lastEffectSchool = school;
        }
    }
}
