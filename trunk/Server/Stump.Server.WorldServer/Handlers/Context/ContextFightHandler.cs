using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Fights;
using Stump.Server.WorldServer.Worlds.Fights.Buffs;
using Stump.Server.WorldServer.Worlds.Fights.Triggers;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler : WorldHandlerContainer
    {
        [WorldHandler(GameActionFightCastRequestMessage.Id)]
        public static void HandleGameActionFightCastRequestMessage(WorldClient client,
                                                                   GameActionFightCastRequestMessage message)
        {
            if (!client.ActiveCharacter.IsFighting())
                return;

            var spell = client.ActiveCharacter.Spells.GetSpell(message.spellId);

            if (spell == null)
                return;

            client.ActiveCharacter.Fighter.CastSpell(spell, client.ActiveCharacter.Map.Cells[message.cellId]);
        }

        [WorldHandler(GameFightTurnFinishMessage.Id)]
        public static void HandleGameFightTurnFinishMessage(WorldClient client, GameFightTurnFinishMessage message)
        {
            if (!client.ActiveCharacter.IsFighting())
                return;

            client.ActiveCharacter.Fight.RequestTurnEnd(client.ActiveCharacter.Fighter);
        }

        [WorldHandler(GameFightTurnReadyMessage.Id)]
        public static void HandleGameFightTurnReadyMessage(WorldClient client, GameFightTurnReadyMessage message)
        {
            client.ActiveCharacter.Fighter.ToggleTurnReady(message.isReady);
        }

        [WorldHandler(GameFightReadyMessage.Id)]
        public static void HandleGameFightReadyMessage(WorldClient client, GameFightReadyMessage message)
        {
            client.ActiveCharacter.Fighter.ToggleReady(message.isReady);
        }

        [WorldHandler(GameContextQuitMessage.Id)]
        public static void HandleGameContextQuitMessage(WorldClient client, GameContextQuitMessage message)
        {
            if (!client.ActiveCharacter.IsFighting())
                return;

            client.ActiveCharacter.Fight.LeaveFight(client.ActiveCharacter.Fighter);
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

        [WorldHandler(GameFightOptionToggleMessage.Id)]
        public static void HandleGameFightOptionToggleMessage(WorldClient client, GameFightOptionToggleMessage message)
        {
            if (!client.ActiveCharacter.IsFighting())
                return;

            if (!client.ActiveCharacter.Fighter.IsTeamLeader())
                return;

            client.ActiveCharacter.Team.ToggleOption((FightOptionsEnum) message.option);
        }

        [WorldHandler(GameFightJoinRequestMessage.Id)]
        public static void HandleGameFightJoinRequestMessage(WorldClient client, GameFightJoinRequestMessage message)
        {
            if (client.ActiveCharacter.IsFighting())
                return;

            var fight = FightManager.Instance.GetFight(message.fightId);

            if (fight.Map != client.ActiveCharacter.Map)
            {
                SendChallengeFightJoinRefusedMessage(client, client.ActiveCharacter, FighterRefusedReasonEnum.WRONG_MAP);
                return;
            }

            FightTeam team;
            if (fight.RedTeam.Leader.Id == message.fighterId)
                team = fight.RedTeam;
            else if (fight.BlueTeam.Leader.Id == message.fighterId)
                team = fight.BlueTeam;
            else
            {
                SendChallengeFightJoinRefusedMessage(client, client.ActiveCharacter, FighterRefusedReasonEnum.WRONG_MAP);
                return;
            }

            FighterRefusedReasonEnum error;
            if (( error = team.CanJoin(client.ActiveCharacter) ) != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
            {
                SendChallengeFightJoinRefusedMessage(client, client.ActiveCharacter, error);
            }
            else
            {
                team.AddFighter(client.ActiveCharacter.CreateFighter(team));
            }
            
        }

        [WorldHandler(GameContextKickMessage.Id)]
        public static void HandleGameContextKickMessage(WorldClient client, GameContextKickMessage message)
        {
            if (!client.ActiveCharacter.IsFighting() ||
                !client.ActiveCharacter.Fighter.IsTeamLeader())
                return;

            var target = client.ActiveCharacter.Fight.GetOneFighter<CharacterFighter>(message.targetId);

            if (target == null)
                return;

            client.ActiveCharacter.Fight.KickPlayer(target);
        }

        public static void SendGameFightStartMessage(WorldClient client)
        {
            client.Send(new GameFightStartMessage());
        }

        public static void SendGameFightStartingMessage(WorldClient client, FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightStartingMessage((sbyte) fightTypeEnum));
        }

        public static void SendGameRolePlayShowChallengeMessage(WorldClient client, Fight fight)
        {
            client.Send(new GameRolePlayShowChallengeMessage(fight.GetFightCommonInformations()));
        }

        public static void SendGameRolePlayRemoveChallengeMessage(WorldClient client, Fight fight)
        {
            client.Send(new GameRolePlayRemoveChallengeMessage(fight.Id));
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

        public static void SendGameFightNewRoundMessage(WorldClient client, int roundNumber)
        {
            client.Send(new GameFightNewRoundMessage(roundNumber));
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

        public static void SendGameFightRemoveTeamMemberMessage(WorldClient client, FightActor fighter)
        {
            client.Send(new GameFightRemoveTeamMemberMessage((short) fighter.Fight.Id, fighter.Team.Id, fighter.Id));
        }

        public static void SendGameFightLeaveMessage(WorldClient client, FightActor fighter)
        {
            client.Send(new GameFightLeaveMessage(fighter.Id));
        }

        public static void SendGameFightPlacementPossiblePositionsMessage(WorldClient client, Fight fight, sbyte team)
        {
            client.Send(new GameFightPlacementPossiblePositionsMessage(
                            fight.RedTeam.PlacementCells.Select(entry => entry.Id),
                            fight.BlueTeam.PlacementCells.Select(entry => entry.Id),
                            team));
        }

        public static void SendGameFightOptionStateUpdateMessage(WorldClient client, FightTeam team, FightOptionsEnum option, bool state)
        {
            client.Send(new GameFightOptionStateUpdateMessage((short) team.Fight.Id, team.Id, (sbyte)option, state));
        }

        public static void SendGameActionFightSpellCastMessage(WorldClient client, ActionsEnum actionId, FightActor caster,
                                                               Cell cell, FightSpellCastCriticalEnum critical, bool silentCast,
                                                               Worlds.Spells.Spell spell)
        {
            client.Send(new GameActionFightSpellCastMessage((short) actionId, caster.Id, cell.Id, (sbyte) (critical),
                                                            silentCast, (short) spell.Id, spell.CurrentLevel));
        }

        public static void SendGameActionFightDispellableEffectMessage(WorldClient client, Buff buff)
        {
            client.Send(new GameActionFightDispellableEffectMessage(buff.Effect.Id, buff.Caster.Id, buff.GetAbstractFightDispellableEffect()));
        }

        public static void SendGameActionFightMarkCellsMessage(WorldClient client, MarkTrigger trigger)
        {
            var action = trigger.Type == GameActionMarkTypeEnum.GLYPH ? ActionsEnum.ACTION_FIGHT_ADD_GLYPH_CASTING_SPELL : ActionsEnum.ACTION_FIGHT_ADD_TRAP_CASTING_SPELL;
            client.Send(new GameActionFightMarkCellsMessage((short)action, trigger.Caster.Id, trigger.GetGameActionMark()));
        }

        public static void SendGameActionFightUnmarkCellsMessage(WorldClient client, MarkTrigger trigger)
        {
            client.Send(new GameActionFightUnmarkCellsMessage(310, trigger.Caster.Id, trigger.Id));
        }

        public static void SendGameActionFightTriggerGlyphTrapMessage(WorldClient client, MarkTrigger trigger, FightActor target, Worlds.Spells.Spell triggeredSpell)
        {
            var action = trigger.Type == GameActionMarkTypeEnum.GLYPH ? ActionsEnum.ACTION_FIGHT_TRIGGER_GLYPH : ActionsEnum.ACTION_FIGHT_TRIGGER_TRAP;
            client.Send(new GameActionFightTriggerGlyphTrapMessage((short)action, trigger.Caster.Id, trigger.Id, target.Id, (short) triggeredSpell.Id));
        }
    }
}