using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Network;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

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

            client.ActiveCharacter.Fighter.PassTurn();
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
            if (client.ActiveCharacter.IsFighting())
                client.ActiveCharacter.Fighter.LeaveFight();
            else if (client.ActiveCharacter.IsSpectator())
                client.ActiveCharacter.Spectator.Leave();
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

            if (!client.ActiveCharacter.Fight.IsStarted)
                client.ActiveCharacter.Team.ToggleOption((FightOptionsEnum) message.option);
            else if (message.option == 0)
                client.ActiveCharacter.Fight.ToggleSpectatorClosed(!client.ActiveCharacter.Fight.SpectatorClosed);
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

            if (fight.IsStarted)
            {
                if (message.fighterId == 0 && fight.CanSpectatorJoin(client.ActiveCharacter))
                {
                    fight.AddSpectator(client.ActiveCharacter.CreateSpectator(fight));
                }
                
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

            client.ActiveCharacter.Fight.KickFighter(target);
        }

        public static void SendGameFightStartMessage(IPacketReceiver client)
        {
            client.Send(new GameFightStartMessage());
        }

        public static void SendGameFightStartingMessage(IPacketReceiver client, FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightStartingMessage((sbyte) fightTypeEnum));
        }

        public static void SendGameRolePlayShowChallengeMessage(IPacketReceiver client, Fight fight)
        {
            client.Send(new GameRolePlayShowChallengeMessage(fight.GetFightCommonInformations()));
        }

        public static void SendGameRolePlayRemoveChallengeMessage(IPacketReceiver client, Fight fight)
        {
            client.Send(new GameRolePlayRemoveChallengeMessage(fight.Id));
        }

        public static void SendGameFightEndMessage(IPacketReceiver client, Fight fight)
        {
            client.Send(new GameFightEndMessage(fight.GetFightDuration(), fight.AgeBonus, new FightResultListEntry[0]));
        }

        public static void SendGameFightEndMessage(IPacketReceiver client, Fight fight, IEnumerable<FightResultListEntry> results)
        {
            client.Send(new GameFightEndMessage(fight.GetFightDuration(), fight.AgeBonus, results));
        }

        public static void SendGameFightJoinMessage(IPacketReceiver client, bool canBeCancelled, bool canSayReady,
                                                    bool isSpectator, bool isFightStarted, int timeMaxBeforeFightStart,
                                                    FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightJoinMessage(canBeCancelled, canSayReady, isSpectator, isFightStarted,
                                                 timeMaxBeforeFightStart, (sbyte) fightTypeEnum));
        }

        public static void SendGameFightSpectateMessage(IPacketReceiver client, Fight fight)
        {
            client.Send(new GameFightSpectateMessage(
                fight.GetBuffs().Select(entry => entry.GetFightDispellableEffectExtendedInformations()),
                fight.GetTriggers().Select(entry => entry.GetHiddenGameActionMark()),
                fight.TimeLine.RoundNumber));    
        }

        public static void SendGameFightTurnResumeMessage(IPacketReceiver client, FightActor playingTurn, int waitTime)
        {
            client.Send(new GameFightTurnResumeMessage(playingTurn.Id, waitTime));
        }

        public static void SendChallengeFightJoinRefusedMessage(IPacketReceiver client, Character character,
                                                                FighterRefusedReasonEnum reason)
        {
            client.Send(new ChallengeFightJoinRefusedMessage(character.Id, (sbyte)reason));
        }

        public static void SendGameFightHumanReadyStateMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightHumanReadyStateMessage(fighter.Id, fighter.IsReady));
        }

        public static void SendGameFightTurnReadyRequestMessage(IPacketReceiver client, FightActor entity)
        {
            client.Send(new GameFightTurnReadyRequestMessage(entity.Id));
        }

        public static void SendGameFightSynchronizeMessage(IPacketReceiver client, Fight fight)
        {
            client.Send(
                new GameFightSynchronizeMessage(
                    fight.GetAllFighters().Select(entry => entry.GetGameFightFighterInformations())));
        }

        public static void SendGameFightNewRoundMessage(IPacketReceiver client, int roundNumber)
        {
            client.Send(new GameFightNewRoundMessage(roundNumber));
        }

        public static void SendGameFightTurnListMessage(IPacketReceiver client, Fight fight)
        {
            client.Send(new GameFightTurnListMessage(fight.GetAliveFightersIds(), fight.GetDeadFightersIds()));
        }

        public static void SendGameFightTurnStartMessage(IPacketReceiver client, int id, int waitTime)
        {
            client.Send(new GameFightTurnStartMessage(id, waitTime));
        }

        public static void SendGameFightTurnFinishMessage(IPacketReceiver client)
        {
            client.Send(new GameFightTurnFinishMessage());
        }

        public static void SendGameFightTurnEndMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightTurnEndMessage(fighter.Id));
        }

        public static void SendGameFightUpdateTeamMessage(IPacketReceiver client, Fight fight, FightTeam team)
        {
            client.Send(new GameFightUpdateTeamMessage(
                            (short) fight.Id,
                            team.GetFightTeamInformations()));
        }

        public static void SendGameFightShowFighterMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightShowFighterMessage(fighter.GetGameFightFighterInformations()));
        }

        public static void SendGameFightRemoveTeamMemberMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightRemoveTeamMemberMessage((short) fighter.Fight.Id, fighter.Team.Id, fighter.Id));
        }

        public static void SendGameFightLeaveMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightLeaveMessage(fighter.Id));
        }

        public static void SendGameFightPlacementPossiblePositionsMessage(IPacketReceiver client, Fight fight, sbyte team)
        {
            client.Send(new GameFightPlacementPossiblePositionsMessage(
                            fight.RedTeam.PlacementCells.Select(entry => entry.Id),
                            fight.BlueTeam.PlacementCells.Select(entry => entry.Id),
                            team));
        }

        public static void SendGameFightOptionStateUpdateMessage(IPacketReceiver client, FightTeam team, FightOptionsEnum option, bool state)
        {
            client.Send(new GameFightOptionStateUpdateMessage((short) team.Fight.Id, team.Id, (sbyte)option, state));
        }

        public static void SendGameActionFightSpellCastMessage(IPacketReceiver client, ActionsEnum actionId, FightActor caster,
                                                               Cell cell, FightSpellCastCriticalEnum critical, bool silentCast,
                                                               Spell spell)
        {
            client.Send(new GameActionFightSpellCastMessage((short) actionId, caster.Id, cell.Id, (sbyte) (critical),
                                                            silentCast, (short) spell.Id, spell.CurrentLevel));
        }

        public static void SendGameActionFightDispellableEffectMessage(IPacketReceiver client, Buff buff)
        {
            client.Send(new GameActionFightDispellableEffectMessage(buff.GetActionId(), buff.Caster.Id, buff.GetAbstractFightDispellableEffect()));
        }

        public static void SendGameActionFightMarkCellsMessage(IPacketReceiver client, MarkTrigger trigger, bool visible = true)
        {
            var action = trigger.Type == GameActionMarkTypeEnum.GLYPH ? ActionsEnum.ACTION_FIGHT_ADD_GLYPH_CASTING_SPELL : ActionsEnum.ACTION_FIGHT_ADD_TRAP_CASTING_SPELL;
            client.Send(new GameActionFightMarkCellsMessage((short)action, trigger.Caster.Id, visible ? trigger.GetGameActionMark() : trigger.GetHiddenGameActionMark()));
        }

        public static void SendGameActionFightUnmarkCellsMessage(IPacketReceiver client, MarkTrigger trigger)
        {
            client.Send(new GameActionFightUnmarkCellsMessage(310, trigger.Caster.Id, trigger.Id));
        }

        public static void SendGameActionFightTriggerGlyphTrapMessage(IPacketReceiver client, MarkTrigger trigger, FightActor target, Spell triggeredSpell)
        {
            var action = trigger.Type == GameActionMarkTypeEnum.GLYPH ? ActionsEnum.ACTION_FIGHT_TRIGGER_GLYPH : ActionsEnum.ACTION_FIGHT_TRIGGER_TRAP;
            client.Send(new GameActionFightTriggerGlyphTrapMessage((short)action, trigger.Caster.Id, trigger.Id, target.Id, (short) triggeredSpell.Id));
        }
    }
}