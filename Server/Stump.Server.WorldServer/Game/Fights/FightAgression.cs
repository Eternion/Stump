using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Results;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Handlers.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights
{
    public class FightAgression : Fight<FightPlayerTeam, FightPlayerTeam>
    {
        public FightAgression(int id, Map fightMap, FightPlayerTeam defendersTeam, FightPlayerTeam challengersTeam)
            : base(id, fightMap, defendersTeam, challengersTeam)
        {
            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartPlacement()
        {
            base.StartPlacement();

            m_placementTimer = Map.Area.CallDelayed(FightConfiguration.PlacementPhaseTime, StartFighting);
        }

        public override void StartFighting()
        {
            m_placementTimer.Dispose();

            base.StartFighting();
        }

        public override FightTypeEnum FightType
        {
            get { return FightTypeEnum.FIGHT_TYPE_AGRESSION; }
        }

        public override bool IsPvP
        {
            get { return true; }
        }

        public override bool IsMultiAccountRestricted
        {
            get { return true; }
        }

        protected override void ApplyResults()
        {
            foreach (var fightResult in Results)
            {
                fightResult.Apply();
            }
        }

        protected override List<IFightResult> GetResults()
        {
            var results = GetFightersAndLeavers().Where(entry => entry.HasResult).
                Select(fighter => fighter.GetFightResult()).ToList();

            foreach (var playerResult in results.OfType<FightPlayerResult>())
            {
                playerResult.SetEarnedHonor(CalculateEarnedHonor(playerResult.Fighter),
                    CalculateEarnedDishonor(playerResult.Fighter));

                CalculateEarnedPevetons(playerResult);
            }

            return results;
        }

        protected override void SendGameFightJoinMessage(CharacterFighter fighter)
        {
            ContextHandler.SendGameFightJoinMessage(fighter.Character.Client, CanCancelFight(), true, IsStarted, (int)GetPlacementTimeLeft().TotalMilliseconds / 100, FightType);
        }

        public override TimeSpan GetPlacementTimeLeft()
        {
            var timeleft = FightConfiguration.PlacementPhaseTime - (DateTime.Now - CreationTime).TotalMilliseconds;

            if (timeleft < 0)
                timeleft = 0;

            return TimeSpan.FromMilliseconds(timeleft);
        }

        protected override bool CanCancelFight() => false;

        private void CalculateEarnedPevetons(FightPlayerResult result)
        {
            var pvpSeek = result.Character.Inventory.GetItems(x => x.Template.Id == (int)ItemIdEnum.ORDRE_DEXECUTION_10085).FirstOrDefault();

            if (pvpSeek != null && ChallengersTeam.Fighters.Contains(result.Fighter))
            {
                if (pvpSeek.Effects.FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Seek) is EffectString seekEffect)
                {
                    var target = result.Fighter.OpposedTeam.GetAllFightersWithLeavers<CharacterFighter>().FirstOrDefault(x => x.Name == seekEffect.Text);

                    if (target != null)
                    {
                        result.Character.Inventory.RemoveItem(pvpSeek);

                        if (Winners == result.Fighter.Team)
                        {
                            result.Loot.AddItem((int)ItemIdEnum.PEVETON_10275, 2);
                            result.Character.SendServerMessage("Vous avez abattu votre cible avec succès, vous avez gagné 2 Pévétons !");
                        }
                        else
                        {
                            target.Loot.AddItem((int)ItemIdEnum.PEVETON_10275, 2);
                            target.Character.SendServerMessage("Vous avez vaincu votre traqueur, vous avez gagné 2 Pévétons !");
                        }
                    }
                }
                else
                    result.Character.Inventory.RemoveItem(pvpSeek);
            }
        }

        public short CalculateEarnedHonor(CharacterFighter character)
        {
            if (Draw)
                return 0;

            if (character.OpposedTeam.AlignmentSide == AlignmentSideEnum.ALIGNMENT_NEUTRAL)
                return 0;

            var winnersLevel = (double)Winners.GetAllFightersWithLeavers<CharacterFighter>().Sum(entry => entry.Level);
            var losersLevel = (double)Losers.GetAllFightersWithLeavers<CharacterFighter>().Sum(entry => entry.Level);
            var maxLosersLevel = winnersLevel + 15;

            var delta = Math.Floor(Math.Sqrt(character.Level) * 10 * ((losersLevel > maxLosersLevel ? maxLosersLevel : losersLevel) / winnersLevel));

            if (Losers == character.Team)
                delta = -delta;

            return (short)delta;
        }

        public short CalculateEarnedDishonor(CharacterFighter character)
        {
            if (Draw)
                return 0;

            return character.OpposedTeam.AlignmentSide != AlignmentSideEnum.ALIGNMENT_NEUTRAL ? (short)0 : (short)1;
        }
    }
}