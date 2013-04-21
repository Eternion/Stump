using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CacheDataExploiter;
using DofusProtocol.D2oClasses;
using DofusProtocol.Enums;
using DofusProtocol.Messages;
using DofusProtocol.Types;
using Sniffer.Modules;

namespace StatsGather
{
    public class PacketHandler : PacketHandlerModule
    {
        private Dictionary<int, Monster> m_monsters;
        private Dictionary<int, SpellLevel> m_spellLevels;
        private Dictionary<int, Spell> m_spells;

        public override void Initialize()
        {
            m_monsters = new D2OReader("Monsters.d2o").ReadObjects<Monster>();
            m_spellLevels = new D2OReader("SpellLevels.d2o").ReadObjects<SpellLevel>();
            m_spells = new D2OReader("Spells.d2o").ReadObjects<Spell>();

            base.Initialize();
        }


        public override string GetName()
        {
            return "Stats Gather";
        }

        public override string GetAuthor()
        {
            return "bouh2";
        }

        public override string GetVersion()
        {
            return "1.0.0";
        }

        private bool m_inFight = false;
        private List<GameFightFighterInformations> m_fighters = new List<GameFightFighterInformations>();
        private GameActionFightSpellCastMessage m_castedSpell;
        private int counter;

        private Dictionary<MonsterGrade, List<double>> m_result = new Dictionary<MonsterGrade, List<double>>();

        public override void Run()
        {
            m_inFight = false;
            m_fighters = new List<GameFightFighterInformations>();
            m_castedSpell = null;
            counter = 0;

            base.Run();
        }

        public override void Stop()
        {
            var resultsByLevel = m_result.GroupBy(entry => entry.Key.level);
            var name =  DateTime.Now.ToString();

            foreach (var levelResult in resultsByLevel)
            {
                var caracteristic = (int)( from entry in levelResult
                                      from subentry in entry.Value
                                      select subentry ).Average();

                File.AppendAllText(name, string.Format("level={0},result={1}\n", levelResult.Key, caracteristic));
            }

            m_inFight = false;
            m_result.Clear();

            base.Stop();
        }

        public override void Handle(Message message, string sender)
        {
            counter++;
            if (message is GameFightStartingMessage)
            {
                m_inFight = true;
                m_fighters.Clear();
            }
            if (message is GameFightShowFighterMessage && m_inFight)
            {
                var msg = message as GameFightShowFighterMessage;
                m_fighters.Add(msg.informations);
            }
            if (message is GameActionFightSpellCastMessage && m_inFight)
            {
                m_castedSpell = message as GameActionFightSpellCastMessage;
                counter = 0;
            }
            if (message is GameActionFightLifePointsVariationMessage && m_inFight)
            {
                var msg = message as GameActionFightLifePointsVariationMessage;

                if (m_fighters != null && m_castedSpell != null && counter < 5)
                {
                    if (msg.sourceId == m_castedSpell.sourceId && msg.sourceId < 0)
                    {
                        var fighter = m_fighters.Where(entry => entry.contextualId == msg.sourceId).FirstOrDefault() as GameFightMonsterInformations;
                        if (fighter != null)
                        {
                            StoreResult(fighter.creatureGenericId, fighter.creatureGrade, m_castedSpell.spellId, m_castedSpell.spellLevel, Math.Abs(msg.delta));
                        }
                    }
                }
            }
            if (message is GameFightEndMessage)
            {
                m_inFight = false;
            }
        }

        private void StoreResult(int monsterId, int gradeId, int spellId, int spellLevelId, int damage)
        {
            if (!m_monsters.ContainsKey(monsterId))
                return;

            var monster = m_monsters[monsterId];
            var grade = monster.grades[gradeId];

            if (!m_spells.ContainsKey(spellId))
                return;

            var spell = m_spells[spellId];
            var spellLevel = m_spellLevels[(int) spell.spellLevels[spellLevelId]];


            var damageEffect = spellLevel.effects.SingleOrDefault(entry => entry.effectId > (int)EffectsEnum.Effect_DamageWater && entry.effectId < (int)EffectsEnum.Effect_DamageNeutral);
        
            if (damageEffect == null)
                return;

            var linearDamage = ( ( damageEffect.diceSide - damageEffect.diceNum ) / 2 ) + damageEffect.diceNum;

            var caracteristic = (damage / (double)linearDamage * 100d) - 100;

            if (caracteristic > 0)
            {
                List<double> output;
                if (!m_result.TryGetValue(grade, out output))
                {
                    m_result.Add(grade, new List<double> { caracteristic });
                }
                else
                {
                    m_result[grade].Add(caracteristic);
                }
            }
        }
    }
}
