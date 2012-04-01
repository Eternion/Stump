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
            if (!client.Character.IsFighting())
                return;

            var spell = client.Character.Spells.GetSpell(message.spellId);

            if (spell == null)
                return;

            client.Character.Fighter.CastSpell(spell, client.Character.Map.Cells[message.cellId]);
        }

        [WorldHandler(GameFightTurnFinishMessage.Id)]
        public static void HandleGameFightTurnFinishMessage(WorldClient client, GameFightTurnFinishMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            client.Character.Fighter.PassTurn();
        }

        [WorldHandler(GameFightTurnReadyMessage.Id)]
        public static void HandleGameFightTurnReadyMessage(WorldClient client, GameFightTurnReadyMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            client.Character.Fighter.ToggleTurnReady(message.isReady);
        }

        [WorldHandler(GameFightReadyMessage.Id)]
        public static void HandleGameFightReadyMessage(WorldClient client, GameFightReadyMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            client.Character.Fighter.ToggleReady(message.isReady);
        }

        [WorldHandler(GameContextQuitMessage.Id)]
        public static void HandleGameContextQuitMessage(WorldClient client, GameContextQuitMessage message)
        {
            if (client.Character.IsFighting())
                client.Character.Fighter.LeaveFight();
            else if (client.Character.IsSpectator())
                client.Character.Spectator.Leave();
        }

        [WorldHandler(GameFightPlacementPositionRequestMessage.Id)]
        public static void HandleGameFightPlacementPositionRequestMessage(WorldClient client,
                                                                          GameFightPlacementPositionRequestMessage
                                                                              message)
        {
            if (client.Character.Fighter.Position.Cell.Id != message.cellId)
            {
                client.Character.Fighter.ChangePrePlacement(client.Character.Map.Cells[message.cellId]);
            }
        }

        [WorldHandler(GameRolePlayPlayerFightRequestMessage.Id)]
        public static void HandleGameRolePlayPlayerFightRequestMessage(WorldClient client,
                                                                       GameRolePlayPlayerFightRequestMessage message)
        {
            var target = client.Character.Map.GetActor<Character>(message.targetId);

            if (message.friendly)
            {
                FighterRefusedReasonEnum reason = client.Character.CanRequestFight(target);
                if (reason != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                {
                    SendChallengeFightJoinRefusedMessage(client, client.Character, reason);
                }
                else
                {
                    var fightRequest = new FightRequest(client.Character, target);

                    client.Character.OpenRequestBox(fightRequest);
                    target.OpenRequestBox(fightRequest);

                    fightRequest.Open();
                }
            }
            else // agression
            {
                FighterRefusedReasonEnum reason = client.Character.CanAgress(target);
                if (reason != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                {
                    SendChallengeFightJoinRefusedMessage(client, client.Character, reason);
                }
                else
                {
                    var fight = FightManager.Instance.CreateAgressionFight(target.Map);

                    fight.RedTeam.AddFighter(client.Character.CreateFighter(fight.RedTeam));
                    fight.BlueTeam.AddFighter(target.CreateFighter(fight.BlueTeam));

                    fight.StartPlacement();
                }
            }
        }

        [WorldHandler(GameRolePlayPlayerFightFriendlyAnswerMessage.Id)]
        public static void HandleGameRolePlayPlayerFightFriendlyAnswerMessage(WorldClient client,
                                                                              GameRolePlayPlayerFightFriendlyAnswerMessage
                                                                                  message)
        {
            if (!client.Character.IsInRequest() ||
                !(client.Character.RequestBox is FightRequest))
                return;

            if (message.accept)
                client.Character.RequestBox.Accept();
            else if (client.Character == client.Character.RequestBox.Target)
                client.Character.RequestBox.Deny();
            else
                client.Character.RequestBox.Cancel();
        }

        [WorldHandler(GameFightOptionToggleMessage.Id)]
        public static void HandleGameFightOptionToggleMessage(WorldClient client, GameFightOptionToggleMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            if (!client.Character.Fighter.IsTeamLeader())
                return;

            if (!client.Character.Fight.IsStarted)
                client.Character.Team.ToggleOption((FightOptionsEnum) message.option);
            else if (message.option == 0)
                client.Character.Fight.ToggleSpectatorClosed(!client.Character.Fight.SpectatorClosed);
        }

        [WorldHandler(GameFightJoinRequestMessage.Id)]
        public static void HandleGameFightJoinRequestMessage(WorldClient client, GameFightJoinRequestMessage message)
        {
            if (client.Character.IsFighting())
                return;

            var fight = FightManager.Instance.GetFight(message.fightId);

            if (fight.Map != client.Character.Map)
            {
                SendChallengeFightJoinRefusedMessage(client, client.Character, FighterRefusedReasonEnum.WRONG_MAP);
                return;
            }

            if (fight.IsStarted)
            {
                if (message.fighterId == 0 && fight.CanSpectatorJoin(client.Character))
                {
                    fight.AddSpectator(client.Character.CreateSpectator(fight));
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
                SendChallengeFightJoinRefusedMessage(client, client.Character, FighterRefusedReasonEnum.WRONG_MAP);
                return;
            }

            FighterRefusedReasonEnum error;
            if (( error = team.CanJoin(client.Character) ) != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
            {
                SendChallengeFightJoinRefusedMessage(client, client.Character, error);
            }
            else
            {
                team.AddFighter(client.Character.CreateFighter(team));
            }
            
        }

        [WorldHandler(GameContextKickMessage.Id)]
        public static void HandleGameContextKickMessage(WorldClient client, GameContextKickMessage message)
        {
            if (!client.Character.IsFighting() ||
                !client.Character.Fighter.IsTeamLeader())
                return;

            var target = client.Character.Fight.GetOneFighter<CharacterFighter>(message.targetId);

            if (target == null)
                return;

            client.Character.Fight.KickFighter(target);
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

        public static void SendGameFightSynchronizeMessage(WorldClient client, Fight fight)
        {
            client.Send(
                new GameFightSynchronizeMessage(
                    fight.GetAllFighters().Select(entry => entry.GetGameFightFighterInformations(client))));
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

        public static void SendGameFightShowFighterMessage(WorldClient client, FightActor fighter)
        {   
            client.Send(new GameFightShowFighterMessage(fighter.GetGameFightFighterInformations(client)));
        }

        public static void SendGameFightRefreshFighterMessage(WorldClient client, FightActor fighter)
        {
            client.Send(new GameFightRefreshFighterMessage(fighter.GetGameFightFighterInformations(client)));
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