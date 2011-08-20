using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        /*[WorldHandler(typeof (GameContextQuitMessage))]
        public static void HandleGameContextQuitMessage(WorldClient client, GameContextQuitMessage message)
        {
            client.ActiveCharacter.Fighter.LeaveFight();
        }

        [WorldHandler(typeof (GameActionFightCastRequestMessage))]
        public static void HandleGameActionFightCastRequestMessage(WorldClient client,
                                                                   GameActionFightCastRequestMessage message)
        {
            client.ActiveCharacter.Fighter.CastSpell(message.spellId, message.cellId);
        }

        [WorldHandler(typeof (GameFightTurnFinishMessage))]
        public static void HandleGameFightTurnFinishMessage(WorldClient client, GameFightTurnFinishMessage message)
        {
            client.ActiveCharacter.Fight.FinishTurn(client.ActiveCharacter.Fighter);
        }

        [WorldHandler(typeof (GameFightTurnReadyMessage))]
        public static void HandleGameFightTurnReadyMessage(WorldClient client, GameFightTurnReadyMessage message)
        {
            if (message.isReady && client.ActiveCharacter.Fight.State == FightState.Fighting &&
                client.ActiveCharacter.Fighter.IsPlaying)
                client.ActiveCharacter.Fight.TurnEndConfirm(client.ActiveCharacter.Fighter);
            else if (client.ActiveCharacter.Fight.State == FightState.PreparePosition)
            {
                client.ActiveCharacter.Fighter.PassTurn();
            }
        }

        [WorldHandler(typeof (GameFightReadyMessage))]
        public static void HandleGameFightReadyMessage(WorldClient client, GameFightReadyMessage message)
        {
            client.ActiveCharacter.Fighter.SetReady(message.isReady);
        }

        [WorldHandler(typeof (GameFightPlacementPositionRequestMessage))]
        public static void HandleGameFightPlacementPositionRequestMessage(WorldClient client,
                                                                          GameFightPlacementPositionRequestMessage
                                                                              message)
        {
            if (client.ActiveCharacter.Fighter.Position.CellId != message.cellId)
            {
                client.ActiveCharacter.Fighter.ChangePrePlacementPosition((ushort) message.cellId);
            }
        }

        [WorldHandler(typeof (GameRolePlayPlayerFightRequestMessage))]
        public static void HandleGameRolePlayPlayerFightRequestMessage(WorldClient client,
                                                                       GameRolePlayPlayerFightRequestMessage message)
        {
            var target = client.ActiveCharacter.Map.Get<Character>(message.targetId);

            FighterRefusedReasonEnum reason = client.ActiveCharacter.CanRequestFight(target);
            if (reason != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
            {
                SendChallengeFightJoinRefusedMessage(client, reason);
                return;
            }

            if (message.friendly)
            {
                var fightRequest = new FightRequest(client.ActiveCharacter, target);

                client.ActiveCharacter.RequestDialog(fightRequest);
                target.RequestDialog(fightRequest);

                fightRequest.StartDialog();
            }
        }

        [WorldHandler(typeof (GameRolePlayPlayerFightFriendlyAnswerMessage))]
        public static void HandleGameRolePlayPlayerFightFriendlyAnswerMessage(WorldClient client,
                                                                              GameRolePlayPlayerFightFriendlyAnswerMessage
                                                                                  message)
        {
            if (message.accept)
                client.ActiveCharacter.RequestBox.AcceptDialog();
            else
                ((FightRequest) client.ActiveCharacter.RequestBox).DeniedDialog(client.ActiveCharacter);
        }

        public static void SendGameFightStartMessage(WorldClient client)
        {
            client.Send(new GameFightStartMessage());
        }

        public static void SendGameFightStartingMessage(WorldClient client, FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightStartingMessage((uint) fightTypeEnum));
        }

        public static void SendGameFightEndMessage(WorldClient client, Fight fight)
        {
            client.Send(new GameFightEndMessage((uint) fight.Duration, fight.AgeBonus, new List<FightResultListEntry>()));
        }

        public static void SendGameFightJoinMessage(WorldClient client, bool canBeCancelled, bool canSayReady,
                                                    bool isSpectator, bool isFightStarted, int timeMaxBeforeFightStart,
                                                    FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightJoinMessage(canBeCancelled, canSayReady, isSpectator, isFightStarted,
                                                 (uint) timeMaxBeforeFightStart, (uint) fightTypeEnum));
        }

        public static void SendChallengeFightJoinRefusedMessage(WorldClient client, FighterRefusedReasonEnum reason)
        {
            SendChallengeFightJoinRefusedMessage(client, client.ActiveCharacter, reason);
        }

        public static void SendChallengeFightJoinRefusedMessage(WorldClient client, Entity entity,
                                                                FighterRefusedReasonEnum reason)
        {
            client.Send(new ChallengeFightJoinRefusedMessage((uint) entity.Id, (int) reason));
        }

        public static void SendGameFightHumanReadyStateMessage(WorldClient client, FightGroupMember fighter)
        {
            client.Send(new GameFightHumanReadyStateMessage((uint) fighter.Entity.Id, fighter.IsReady));
        }

        public static void SendGameFightTurnReadyRequestMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameFightTurnReadyRequestMessage((int) entity.Id));
        }

        public static void SendGameFightSynchronizeMessage(WorldClient client, Fight fight)
        {
            client.Send(
                new GameFightSynchronizeMessage(
                    fight.GetAllFighters().Select(entry => entry.Entity.ToNetworkFighter()).ToList()));
        }

        public static void SendGameFightTurnListMessage(WorldClient client, Fight fight)
        {
            client.Send(new GameFightTurnListMessage(fight.GetAlivesIds().ToList(), fight.GetDeadsIds().ToList()));
        }

        public static void SendGameFightTurnStartMessage(WorldClient client, int id, uint waitTime)
        {
            client.Send(new GameFightTurnStartMessage(id, waitTime));
        }

        public static void SendGameFightTurnFinishMessage(WorldClient client)
        {
            client.Send(new GameFightTurnFinishMessage());
        }

        public static void SendGameFightTurnEndMessage(WorldClient client, Entity entity)
        {
            client.Send(new GameFightTurnEndMessage((int) entity.Id));
        }

        public static void SendGameFightUpdateTeamMessage(WorldClient client, Fight fight, FightGroup team)
        {
            client.Send(new GameFightUpdateTeamMessage(
                            (uint) fight.Id,
                            team.ToNetworkFightTeam()));
        }

        public static void SendGameFightShowFighterMessage(WorldClient client, FightGroupMember fighter)
        {
            client.Send(new GameFightShowFighterMessage(fighter.Entity.ToNetworkFighter()));
        }

        public static void SendGameFightPlacementPossiblePositionsMessage(WorldClient client, Fight fight, int team)
        {
            client.Send(new GameFightPlacementPossiblePositionsMessage(
                            fight.SourceGroup.Positions.Select(entry => (uint) entry).ToList(),
                            fight.TargetGroup.Positions.Select(entry => (uint) entry).ToList(),
                            (uint) team));
        }

        public static void SendGameActionFightSpellCastMessage(WorldClient client, uint actionId, LivingEntity source,
                                                               ushort cellId, bool critical, bool silentCast,
                                                               SpellLevel spell)
        {
            client.Send(new GameActionFightSpellCastMessage(actionId, (int) source.Id, cellId, (uint) (critical ? 1 : 0),
                                                            silentCast, (uint) spell.Spell.Id, (uint) spell.Level));
        }*/
    }
}