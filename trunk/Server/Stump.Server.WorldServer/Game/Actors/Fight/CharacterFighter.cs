using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Results.Data;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class CharacterFighter : NamedFighter
    {
        private short m_damageTakenBeforeFight;
        private int m_earnedExp;
        private int m_criticalWeaponBonus;
        private bool m_isUsingWeapon;


        public CharacterFighter(Character character, FightTeam team)
            : base(team)
        {
            Character = character;
            Look = Character.Look.Copy();

            Cell cell;
            Fight.FindRandomFreeCell(this, out cell, false);
            Position = new ObjectPosition(character.Map, cell, character.Direction);

            InitializeCharacterFighter();
        }

        private void InitializeCharacterFighter()
        {
            m_damageTakenBeforeFight = Stats.Health.Context;

            if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                Stats.Health.Context = 0;
        }

        public Character Character
        {
            get;
            private set;
        }

        public ReadyChecker PersonalReadyChecker
        {
            get;
            set;
        }

        public override int Id
        {
            get { return Character.Id; }
        }

        public override string Name
        {
            get { return Character.Name; }
        }

        public override EntityLook Look
        {
            get;
            set;
        }

        public override ObjectPosition MapPosition
        {
            get
            {
                return Character.Position; 
            }
        }

        public override byte Level
        {
            get
            {
                return Character.Level;
            }
        }

        public override StatsFields Stats
        {
            get { return Character.Stats; }
        }

        public override ObjectPosition GetLeaderBladePosition()
        {
            return Character.GetPositionBeforeMove();
        }

        public void ToggleTurnReady(bool ready)
        {
            if (PersonalReadyChecker != null)
                PersonalReadyChecker.ToggleReady(this, ready);

            else if (Fight.ReadyChecker != null)
                Fight.ReadyChecker.ToggleReady(this, ready);
        }

        public override void CastSpell(Spell spell, Cell cell)
        {
            // weapon attack
            if (spell.Id == 0 &&
                Character.Inventory.GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON) != null)
            {
                var weapon = Character.Inventory.GetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON).Template as WeaponTemplate;

                if (weapon == null || !CanUseWeapon(cell, weapon))
                    return;

                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

                var random = new AsyncRandom();
                var critical = RollCriticalDice(weapon);

                if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                {
                    OnWeaponUsed(weapon, cell, critical, false);
                    UseAP((short)weapon.ApCost);
                    Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

                    PassTurn();

                    return;
                }
                if (critical == FightSpellCastCriticalEnum.CRITICAL_HIT)
                    m_criticalWeaponBonus = weapon.CriticalHitBonus;

                m_isUsingWeapon = true;
                var effects = weapon.Effects.Where(entry => !EffectManager.Instance.IsRandomableItemEffect(entry.EffectId)).OfType<EffectDice>();
                var handlers = new List<SpellEffectHandler>();
                foreach (var effect in effects)
                {
                    if (effect.Random > 0)
                    {
                        if (random.NextDouble() > effect.Random / 100d)
                        {
                            // effect ignored
                            continue;
                        }
                    }

                    var handler = EffectManager.Instance.GetSpellEffectHandler(effect, this, spell, cell, critical == FightSpellCastCriticalEnum.CRITICAL_HIT);
                    handlers.Add(handler);
                }

                var silentCast = handlers.Any(entry => entry.RequireSilentCast());

                OnWeaponUsed(weapon, cell, critical, silentCast);
                UseAP((short)weapon.ApCost);

                foreach (var handler in handlers)
                    handler.Apply();

                Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

                m_isUsingWeapon = false;
                m_criticalWeaponBonus = 0;

                // is it the right place to do that ?
                Fight.CheckFightEnd();
            }
            else
            {
                base.CastSpell(spell, cell);
            }
        }


        public override short CalculateDamage(short damage, EffectSchoolEnum type)
        {
            return (short) (base.CalculateDamage(damage, type) + (m_isUsingWeapon ? m_criticalWeaponBonus + Stats[PlayerFields.WeaponDamageBonus] : 0));
        }

        public bool CanUseWeapon(Cell cell, WeaponTemplate weapon)
        {
            if (!IsFighterTurn())
                return false;

            var point = new MapPoint(cell);

            if (point.DistanceToCell(Position.Point) > weapon.WeaponRange ||
                point.DistanceToCell(Position.Point) < weapon.MinRange)
                return false;

            if (AP < weapon.ApCost)
                return false;

            // todo : check Los

            return true;
        }

        public override bool CanCastSpell(Spell spell, Cell cell)
        {
            if (!base.CanCastSpell(spell, cell))
                return false;

            if (!Character.Spells.HasSpell(spell.Id))
                return false;

            return true;
        }

        public FightSpellCastCriticalEnum RollCriticalDice(WeaponTemplate weapon)
        {
            var random = new AsyncRandom();

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (weapon.CriticalHitProbability != 0 && random.Next(weapon.CriticalFailureProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (weapon.CriticalHitProbability != 0 && random.Next((int)CalculateCriticRate(weapon.CriticalHitProbability)) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return critical;
        }

        public void SetEarnedExperience(int experience)
        {
            m_earnedExp = experience;
        }

        public override void ResetFightProperties()
        {
            base.ResetFightProperties();

            if (Fight is FightDuel)
                Stats.Health.DamageTaken = m_damageTakenBeforeFight;
            else if (Stats.Health.Total <= 0)
                Stats.Health.DamageTaken = (short) (Stats.Health.TotalMax - 1);
        }

        public override IFightResult GetFightResult()
        {
            if (m_earnedExp > 0)
            {
                var expData = new FightExperienceData(Character)
                                  {
                                      ExperienceFightDelta = m_earnedExp,
                                      ShowExperience = true,
                                      ShowExperienceFightDelta = true,
                                      ShowExperienceLevelFloor = true,
                                      ShowExperienceNextLevelFloor = true
                                  };

                return new FightPlayerResult(this, GetFighterOutcome(), Loot, expData);
            }

            return new FightPlayerResult(this, GetFighterOutcome(), Loot);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberCharacterInformations(Id, Name, Character.Level);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightCharacterInformations(Id,
                Look,
                GetEntityDispositionInformations(client), 
                Team.Id, 
                IsAlive(),
                GetGameFightMinimalStats(client),
                Name, 
                Character.Level, 
                Character.GetActorAlignmentInformations());
        }
        public override string ToString()
        {
            return Character.ToString();
        }
    }
}