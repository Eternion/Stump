using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Fights;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        /*[WorldHandler(GameContextQuitMessage.Id)]
        public static void HandleGameContextQuitMessage(WorldClient client, GameContextQuitMessage message)
        {
            client.ActiveCharacter.Fighter.LeaveFight();
        }

        [WorldHandler(GameActionFightCastRequestMessage.Id)]
        public static void HandleGameActionFightCastRequestMessage(WorldClient client,
                                                                   GameActionFightCastRequestMessage message)
        {
            client.ActiveCharacter.Fighter.CastSpell(message.spellId, message.cellId);
        }

        [WorldHandler(GameFightTurnFinishMessage.Id)]
        public static void HandleGameFightTurnFinishMessage(WorldClient client, GameFightTurnFinishMessage message)
        {
            client.ActiveCharacter.Fight.FinishTurn(client.ActiveCharacter.Fighter);
        }

        [WorldHandler(GameFightTurnReadyMessage.Id)]
        public static void HandleGameFightTurnReadyMessage(WorldClient client, GameFightTurnReadyMessage message)
        {
            if (message.isReady && client.ActiveCharacter.Fight.State == FightState.Fighting &&
                client.ActiveCharacter.Fighter.IsPlaying)
                client.ActiveCharacter.Fight.TurnEndConfirm(client.ActiveCharacter.Fighter);
            else if (client.ActiveCharacter.Fight.State == FightState.PreparePosition)
            {
                client.ActiveCharacter.Fighter.PassTurn();
            }
        }*/

        [WorldHandler(GameFightReadyMessage.Id)]
        public static void HandleGameFightReadyMessage(WorldClient client, GameFightReadyMessage message)
        {
            client.ActiveCharacter.Fighter.ToggleReady(message.isReady);
        }

        [WorldHandler(GameFightPlacementPositionRequestMessage.Id)]
        public static void HandleGameFightPlacementPositionRequestMessage(WorldClient client,
                                                                          GameFightPlacementPositionRequestMessage
                                                                              message)
        {
            if (client.ActiveCharacter.Fighter.Position.Cell.Id != message.cellId)
            {
                client.ActiveCharacter.Fighter.ChangePrePlacement(client.ActiveCharacter.Map.Cells[message.cellId]);
            }
        }

        [WorldHandler(GameRolePlayPlayerFightRequestMessage.Id)]
        public static void HandleGameRolePlayPlayerFightRequestMessage(WorldClient client,
                                                                       GameRolePlayPlayerFightRequestMessage message)
        {
            var target = client.ActiveCharacter.Map.GetActor<Character>(message.targetId);

            FighterRefusedReasonEnum reason = client.ActiveCharacter.CanRequestFight(target);
            if (reason != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
            {
                SendChallengeFightJoinRefusedMessage(client, client.ActiveCharacter, reason);
            }
            else if (message.friendly)
            {
                var fightRequest = new FightRequest(client.ActiveCharacter, target);

                client.ActiveCharacter.OpenRequestBox(fightRequest);
                target.OpenRequestBox(fightRequest);

                fightRequest.Open();
            }
        }

        [WorldHandler(GameRolePlayPlayerFightFriendlyAnswerMessage.Id)]
        public static void HandleGameRolePlayPlayerFightFriendlyAnswerMessage(WorldClient client,
                                                                              GameRolePlayPlayerFightFriendlyAnswerMessage
                                                                                  message)
        {
            if (!client.ActiveCharacter.IsInRequest() ||
                !(client.ActiveCharacter.RequestBox is FightRequest))
                return;

            if (message.accept)
                client.ActiveCharacter.RequestBox.Accept();
            else if (client.ActiveCharacter == client.ActiveCharacter.RequestBox.Target)
                client.ActiveCharacter.RequestBox.Deny();
            else
                client.ActiveCharacter.RequestBox.Cancel();
        }

        public static void SendGameFightStartMessage(WorldClient client)
        {
            client.Send(new GameFightStartMessage());
        }

        public static void SendGameFightStartingMessage(WorldClient client, FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightStartingMessage((sbyte) fightTypeEnum));
        }

        public static void SendGameFightEndMessage(WorldClient client, Fight fight)
        {
            client.Send(new GameFightEndMessage(fight.GetFightDuration(), fight.AgeBonus, new List<FightResultListEntry>()));
        }

        public static void SendGameFightJoinMessage(WorldClient client, bool canBeCancelled, bool canSayReady,
                                                    bool isSpectator, bool isFightStarted, int timeMaxBeforeFightStart,
                                                    FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightJoinMessage(canBeCancelled, canSayReady, isSpectator, isFightStarted,
                                                 timeMaxBeforeFightStart, (sbyte) fightTypeEnum));
        }

        public static void SendChallengeFightJoinRefusedMessage(WorldClient client, Character character,
                                                                FighterRefusedReasonEnum reason)
        {
            client.Send(new ChallengeFightJoinRefusedMessage(character.Id, (sbyte)reason));
        }

        public static void SendGameFightHumanReadyStateMessage(WorldClient client, FightActor fighter)
        {
            client.Send(new GameFightHumanReadyStateMessage(fighter.Id, fighter.IsReady));
        }

        public static void SendGameFightTurnReadyRequestMessage(WorldClient client, FightActor entity)
        {
            client.Send(new GameFightTurnReadyRequestMessage(entity.Id));
        }

        public static void SendGameFightSynchronizeMessage(WorldClient client, Fight fight)
        {
            client.Send(
                new GameFightSynchronizeMessage(
                    fight.GetAllFighters().Select(entry => entry.GetGameFightFighterInformations())));
        }

        public static void SendGameFightTurnListMessage(WorldClient client, Fight fight)
        {
            client.Send(new GameFightTurnListMessage(fight.GetAliveFightersIds(), fight.GetDeadFightersIds()));
        }

        public static void SendGameFightTurnStartMessage(WorldClient client, int id, int waitTime)
        {
            client.Send(new GameFightTurnStartMessage(id, waitTime));
        }

        public static void SendGameFightTurnFinishMessage(WorldClient client)
        {
            client.Send(new GameFightTurnFinishMessage());
        }

        public static void SendGameFightTurnEndMessage(WorldClient client, FightActor fighter)
        {
            client.Send(new GameFightTurnEndMessage(fighter.Id));
        }

        public static void SendGameFightUpdateTeamMessage(WorldClient client, Fight fight, FightTeam team)
        {
            client.Send(new GameFightUpdateTeamMessage(
                            (short) fight.Id,
                            team.GetFightTeamInformations()));
        }

        public static void SendGameFightShowFighterMessage(WorldClient client, FightActor fighter)
        {
            client.Send(new GameFightShowFighterMessage(fighter.GetGameFightFighterInformations()));
        }

        public static void SendGameFightPlacementPossiblePositionsMessage(WorldClient client, Fight fight, sbyte team)
        {
            client.Send(new GameFightPlacementPossiblePositionsMessage(
                            fight.RedTeam.PlacementCells.Select(entry => entry.Id),
                            fight.BlueTeam.PlacementCells.Select(entry => entry.Id),
                            team));
        }

        /*public static void SendGameActionFightSpellCastMessage(WorldClient client, uint actionId, LivingEntity source,
                                                               ushort cellId, bool critical, bool silentCast,
                                                               SpellLevel spell)
        {
            client.Send(new GameActionFightSpellCastMessage(actionId, (int) source.Id, cellId, (uint) (critical ? 1 : 0),
                                                            silentCast, (uint) spell.Spell.Id, (uint) spell.Level));
        }*/
    }
}