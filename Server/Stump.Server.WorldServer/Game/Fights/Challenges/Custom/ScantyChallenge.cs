using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Custom;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Fights.Challenges.Custom
{
    [ChallengeIdentifier((int)ChallengeEnum.ÉCONOME)]
    public class ScantyChallenge : DefaultChallenge
    {
        private readonly List<FightActor> m_weaponsUsed = new List<FightActor>();

        public ScantyChallenge(int id, IFight fight)
            : base(id, fight)
        {
            BonusMin = 160;
            BonusMax = 220;
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.SpellCasting += OnSpellCasting;
                fighter.WeaponUsed += OnWeaponUsed;
            }
        }

        void OnWeaponUsed(FightActor fighter, WeaponTemplate weapon, Cell cell, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                return;

            if (m_weaponsUsed.Contains(fighter))
                UpdateStatus(ChallengeStatusEnum.FAILED);
            else
                m_weaponsUsed.Add(fighter);
        }

        void OnSpellCasting(FightActor caster, SpellCastHandler spellCastHandler, Cell target, FightSpellCastCriticalEnum critical, bool silentCast)
        {
            if (critical == FightSpellCastCriticalEnum.CRITICAL_FAIL)
                return;

            var entries = caster.SpellHistory.GetEntries(x => x.Spell.SpellId == spellCastHandler.Spell.Id);
            if (!entries.Any())
                return;

            UpdateStatus(ChallengeStatusEnum.FAILED);
        }

        protected override void OnWinnersDetermined(IFight fight, FightTeam winners, FightTeam losers, bool draw)
        {
            base.OnWinnersDetermined(fight, winners, losers, draw);

            foreach (var fighter in Fight.GetAllFighters<CharacterFighter>())
            {
                fighter.SpellCasting -= OnSpellCasting;
                fighter.WeaponUsed -= OnWeaponUsed;
            }
        }
    }
}
