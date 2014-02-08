using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Core.Threading;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.Stats;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Results.Data;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Game.Maps.Cells.Shapes;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Basic;
using FightResultAdditionalData = Stump.Server.WorldServer.Game.Fights.Results.Data.FightResultAdditionalData;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using Stump.Server.WorldServer.Game.Guilds;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public sealed class CharacterFighter : NamedFighter
    {
        private int m_criticalWeaponBonus;
        private int m_damageTakenBeforeFight;
        private short m_earnedDishonor;
        private int m_earnedExp;
        private int m_guildEarnedExp;
        private short m_earnedHonor;
        private bool m_isUsingWeapon;


        public CharacterFighter(Character character, FightTeam team)
            : base(team)
        {
            Character = character;
            Look = Character.Look.Clone();
            Look.RemoveAuras();

            Cell cell;
            if (!Fight.FindRandomFreeCell(this, out cell, false))
                return;

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

        public override ActorLook Look
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

        public bool IsDisconnected
        {
            get;
            private set;
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
            if (!IsFighterTurn())
                return false;

            // weapon attack
            if (spell.Id != 0 ||
                Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON) == null)
                return base.CastSpell(spell, cell);
            var weapon =
                Character.Inventory.TryGetItem(CharacterInventoryPositionEnum.ACCESSORY_POSITION_WEAPON).Template as
                    WeaponTemplate;

            if (weapon == null || !CanUseWeapon(cell, weapon))
                return false;

            Fight.StartSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

            var random = new AsyncRandom();
            var critical = RollCriticalDice(weapon);

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
            var effects =
                weapon.Effects.Where(entry => EffectManager.Instance.IsUnRandomableWeaponEffect(entry.EffectId)).OfType<EffectDice>();
            var handlers = new List<SpellEffectHandler>();
            foreach (var effect in effects)
            {
                if (effect.Random > 0)
                {
                    if (random.NextDouble() > effect.Random/100d)
                    {
                        // effect ignored
                        continue;
                    }
                }

                var handler = EffectManager.Instance.GetSpellEffectHandler(effect, this, spell, cell,
                    critical ==
                    FightSpellCastCriticalEnum
                        .CRITICAL_HIT);
                handler.EffectZone = new Zone(weapon.Type.ZoneShape, (byte) weapon.Type.ZoneSize,
                    handler.CastPoint.OrientationTo(handler.TargetedPoint));
                handler.Targets = SpellTargetType.ENEMY_ALL | SpellTargetType.ALLY_ALL;
                handlers.Add(handler);
            }

            var silentCast = handlers.Any(entry => entry.RequireSilentCast());

            OnWeaponUsed(weapon, cell, critical, silentCast);
            UseAP((short) weapon.ApCost);

            foreach (var handler in handlers)
                handler.Apply();

            Fight.EndSequence(SequenceTypeEnum.SEQUENCE_WEAPON);

            m_isUsingWeapon = false;
            m_criticalWeaponBonus = 0;

            // is it the right place to do that ?
            Fight.CheckFightEnd();

            return true;
        }

        public override SpellCastResult CanCastSpell(Spell spell, Cell cell)
        {
             var result = base.CanCastSpell(spell, cell);

            if (result == SpellCastResult.OK)
                return result;

            switch (result)
            {
                case SpellCastResult.NO_LOS:
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 174);
                    break;
                case SpellCastResult.HAS_NOT_SPELL:
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 169);
                    break;
                case SpellCastResult.NOT_ENOUGH_AP:
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 170, AP, spell.CurrentSpellLevel.ApCost);
                    break;
                case SpellCastResult.UNWALKABLE_CELL:
                case SpellCastResult.CELL_NOT_FREE:
                    Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 172);
                    break;
                default:
                    BasicHandler.SendTextInformationMessage(Character.Client,
                        TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 175);
                    Character.SendServerMessage("(" + result + ")", Color.Red);
                    break;
            }

            return result;
        }


        public override int CalculateDamage(int damage, EffectSchoolEnum type)
        {
            if (Character.GodMode)
                return short.MaxValue;

            return
                base.CalculateDamage(
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

            return AP >= weapon.ApCost && Fight.CanBeSeen(cell, Position.Cell);
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

            var critical = FightSpellCastCriticalEnum.NORMAL;

            if (weapon.CriticalHitProbability != 0 && random.Next(weapon.CriticalFailureProbability) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_FAIL;

            else if (weapon.CriticalHitProbability != 0 &&
                     random.Next((int) CalculateCriticRate(weapon.CriticalHitProbability)) == 0)
                critical = FightSpellCastCriticalEnum.CRITICAL_HIT;

            return critical;
        }

        public override void ResetFightProperties()
        {
            base.ResetFightProperties();

            if (Fight is FightDuel)
                Stats.Health.DamageTaken = m_damageTakenBeforeFight;
            else if (Stats.Health.Total <= 0)
                Stats.Health.DamageTaken = (short) (Stats.Health.TotalMax - 1);
        }

        public void EnterDisconnectedState()
        {
            IsDisconnected = true;
        }

        public override IFightResult GetFightResult()
        {
            return new FightPlayerResult(this, GetFighterOutcome(), Loot);
        }

        public override FightTeamMemberInformations GetFightTeamMemberInformations()
        {
            return new FightTeamMemberCharacterInformations(Id, Name, Character.Level);
        }

        public override GameFightFighterInformations GetGameFightFighterInformations(WorldClient client = null)
        {
            return new GameFightCharacterInformations(Id,
                                                      Look.GetEntityLook(),
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
            if (!Character.GodMode)
                return base.UseAP(amount);

            base.UseAP(amount);
            RegainAP(amount);

            return true;
        }

        public override bool UseMP(short amount)
        {
            return Character.GodMode || base.UseMP(amount);
        }

        public override bool LostAP(short amount)
        {
            if (!Character.GodMode)
                return base.LostAP(amount);

            base.LostAP(amount);
            RegainAP(amount);

            return true;
        }

        public override bool LostMP(short amount)
        {
            return Character.GodMode || base.LostMP(amount);
        }


        public override int InflictDirectDamage(int damage, FightActor from)
        {
            if (!Character.GodMode)
                return base.InflictDirectDamage(damage, @from);

            TriggerBuffs(BuffTriggerType.BEFORE_ATTACKED, damage);
            OnDamageReducted(@from, damage);
            TriggerBuffs(BuffTriggerType.AFTER_ATTACKED, damage);

            return 0;
        }
        #endregion
    }
}