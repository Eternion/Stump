using System.Collections.Generic;
using System.Linq;
using Stump.Core.Attributes;
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
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Results.Data;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Basic;
using FightResultAdditionalData = Stump.Server.WorldServer.Game.Fights.Results.Data.FightResultAdditionalData;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class CharacterFighter : NamedFighter
    {
        private int m_criticalWeaponBonus;
        private short m_damageTakenBeforeFight;
        private short m_earnedDishonor;
        private int m_earnedExp;
        private short m_earnedHonor;
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
            get { return Character.Position; }
        }

        public override byte Level
        {
            get { return Character.Level; }
        }

        public override StatsFields Stats
        {
            get { return Character.Stats; }
        }

        private void InitializeCharacterFighter()
        {
            m_damageTakenBeforeFight = Stats.Health.DamageTaken;

            if (Fight.FightType == FightTypeEnum.FIGHT_TYPE_CHALLENGE)
                Stats.Health.DamageTaken = 0;
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

        public override bool CastSpell(Spell spell, Cell cell)
        {
            // weapon attack
            if (spell.Id == 0 &&
                Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON) != null)
            {
                var weapon =
                    Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON).Template as
                    WeaponTemplate;

                if (weapon == null || !CanUseWeapon(cell, weapon))
                    return false;

                Fight.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

                var random = new AsyncRandom();
                FightSpellCastCriticalEnum critical = RollCriticalDice(weapon);

                if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                {
                    OnWeaponUsed(weapon, cell, critical, false);
                    UseAP((short) weapon.ApCost);
                    Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

                    PassTurn();

                    return false;
                }
                if (critical == FightSpellCastCriticalEnum.CRITICAL_HIT)
                    m_criticalWeaponBonus = weapon.CriticalHitBonus;

                m_isUsingWeapon = true;
                IEnumerable<EffectDice> effects =
                    weapon.Effects.Where(entry => EffectManager.Instance.IsUnRandomableWeaponEffect(entry.EffectId)).OfType<EffectDice>();
                var handlers = new List<SpellEffectHandler>();
                foreach (EffectDice effect in effects)
                {
                    if (effect.Random > 0)
                    {
                        if (random.NextDouble() > effect.Random/100d)
                        {
                            // effect ignored
                            continue;
                        }
                    }

                    SpellEffectHandler handler = EffectManager.Instance.GetSpellEffectHandler(effect, this, spell, cell,
                                                                                              critical ==
                                                                                              FightSpellCastCriticalEnum
                                                                                                  .CRITICAL_HIT);
                    handlers.Add(handler);
                }

                bool silentCast = handlers.Any(entry => entry.RequireSilentCast());

                OnWeaponUsed(weapon, cell, critical, silentCast);
                UseAP((short) weapon.ApCost);

                foreach (SpellEffectHandler handler in handlers)
                    handler.Apply();

                Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

                m_isUsingWeapon = false;
                m_criticalWeaponBonus = 0;

                // is it the right place to do that ?
                Fight.CheckFightEnd();

                return true;
            }

            return base.CastSpell(spell, cell);
        }

        public override bool CanCastSpell(Spell spell, Cell cell)
        {
             bool can = base.CanCastSpell(spell, cell);

             if (!can)
             {
                 // cannot cast spell msg
                 BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 175);
             }

             return can;
        }


        public override short CalculateDamage(short damage, EffectSchoolEnum type)
        {
            if (Character.GodMode)
                return short.MaxValue;

            return
                base.CalculateDamage(
                    (short)
                    ((m_isUsingWeapon ? m_criticalWeaponBonus + Stats[PlayerFields.WeaponDamageBonus] : 0) + damage),
                    type);
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

        public override Spell GetSpell(int id)
        {
            return Character.Spells.GetSpell(id);
        }

        public override bool HasSpell(int id)
        {
            return Character.Spells.HasSpell(id);
        }

        public FightSpellCastCriticalEnum RollCriticalDice(WeaponTemplate weapon)
        {
            var random = new AsyncRandom();

            FightSpellCastCriticalEnum critical = FightSpellCastCriticalEnum.NORMAL;

            if (weapon.CriticalHitProbability != 0 && random.Next(weapon.CriticalFailureProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (weapon.CriticalHitProbability != 0 &&
                     random.Next((int) CalculateCriticRate(weapon.CriticalHitProbability)) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return critical;
        }

        public void SetEarnedExperience(int experience)
        {
            m_earnedExp = experience;
        }

        public void SetEarnedHonor(short honor)
        {
            m_earnedHonor = honor;
        }

        public void SetEarnedDishonor(short dishonor)
        {
            m_earnedDishonor = dishonor;
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
            var additionalDatas = new List<FightResultAdditionalData>();

            if (m_earnedExp != 0)
            {
                additionalDatas.Add(new FightExperienceData(Character)
                                        {
                                            ExperienceFightDelta = m_earnedExp,
                                            ShowExperience = true,
                                            ShowExperienceFightDelta = true,
                                            ShowExperienceLevelFloor = true,
                                            ShowExperienceNextLevelFloor = true,
                                        });
            }

            if (m_earnedHonor != 0 || m_earnedDishonor != 0)
            {
                additionalDatas.Add(new FightPvpData(Character)
                                        {
                                            HonorDelta = m_earnedHonor,
                                            DishonorDelta = m_earnedDishonor,
                                            Honor = Character.Honor,
                                            Dishonor = Character.Dishonor,
                                            Grade = (byte) Character.AlignmentGrade,
                                            MinHonorForGrade = Character.LowerBoundHonor,
                                            MaxHonorForGrade = Character.UpperBoundHonor,
                                        });
            }

            return new FightPlayerResult(this, GetFighterOutcome(), Loot, additionalDatas.ToArray());
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
                                                      Character.GetActorAlignmentInformations(),
                                                      (sbyte) Character.Breed.Id);
        }

        public override string ToString()
        {
            return Character.ToString();
        }

        #region God state
        public override bool UseAP(short amount)
        {
            if (Character.GodMode)
            {
                base.UseAP(amount);
                RegainAP(amount);

                return true;
            }

            return base.UseAP(amount);
        }

        public override bool UseMP(short amount)
        {
            if (Character.GodMode)
                return true;

            return base.UseMP(amount);
        }

        public override bool LostAP(short amount)
        {
            if (Character.GodMode)
            {
                base.LostAP(amount);
                RegainAP(amount);

                return true;
            }

            return base.LostAP(amount);
        }

        public override bool LostMP(short amount)
        {
            if (Character.GodMode)
                return true;

            return base.LostMP(amount);
        }


        public override short CalculateArmorReduction(EffectSchoolEnum damageType)
        {
            if (Character.GodMode)
                return short.MaxValue;

            return base.CalculateArmorReduction(damageType);
        }
        #endregion
    }
}