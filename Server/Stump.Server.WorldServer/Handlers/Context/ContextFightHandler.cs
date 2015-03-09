using System.Collections.Generic;
using System.Linq;
using NLog.Targets;
using Stump.Core.Reflection;
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
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Fights.Triggers;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Handlers.Context
{
    public partial class ContextHandler
    {
        [WorldHandler(GameActionFightCastRequestMessage.Id)]
        public static void HandleGameActionFightCastRequestMessage(WorldClient client, GameActionFightCastRequestMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            var fighter = client.Character.Fighter;

            var spell = fighter.IsSlaveTurn() ?
                fighter.GetSlave().GetSpell(message.spellId) : fighter.GetSpell(message.spellId);

            if (spell == null)
                return;

            if (fighter.IsSlaveTurn())
                fighter.GetSlave().CastSpell(spell, client.Character.Fight.Map.Cells[message.cellId]);
            else
                fighter.CastSpell(spell, client.Character.Fight.Map.Cells[message.cellId]);
        }

        [WorldHandler(GameActionFightCastOnTargetRequestMessage.Id)]
        public static void HandleGameActionFightCastOnTargetRequestMessage(WorldClient client, GameActionFightCastOnTargetRequestMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            var fighter = client.Character.Fighter;

            var spell = fighter.IsSlaveTurn() ?
                fighter.GetSlave().GetSpell(message.spellId) : fighter.GetSpell(message.spellId);

            if (spell == null)
                return;

            var target = client.Character.Fight.GetOneFighter(message.targetId);

            if (target == null)
                return;

            if (target.GetVisibleStateFor(fighter) == GameActionFightInvisibilityStateEnum.INVISIBLE)
            {
                //Impossible de lancer ce sort : la cellule visée n'est pas valide !
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 193);
                return;
            }

            if (fighter.IsSlaveTurn())
                fighter.GetSlave().CastSpell(spell, target.Cell);
            else
                fighter.CastSpell(spell, target.Cell);
        }


        [WorldHandler(GameFightTurnFinishMessage.Id)]
        public static void HandleGameFightTurnFinishMessage(WorldClient client, GameFightTurnFinishMessage message)
        {
            if (!client.Character.IsFighting())
                return;

            if (client.Character.Fighter.IsSlaveTurn())
            {
                client.Character.Fighter.GetSlave().PassTurn();
                return;
            }

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

            if (client.Character.Fight is FightPvT)
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
                client.Character.Fighter.ChangePrePlacement(client.Character.Fight.Map.Cells[message.cellId]);
            }
        }

        [WorldHandler(GameRolePlayPlayerFightRequestMessage.Id)]
        public static void HandleGameRolePlayPlayerFightRequestMessage(WorldClient client, GameRolePlayPlayerFightRequestMessage message)
        {
            var target = client.Character.Map.GetActor<Character>(message.targetId);

            if (target == null)
                return;

            if (message.friendly)
            {
                var reason = client.Character.CanRequestFight(target);
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
                var reason = client.Character.CanAgress(target);
                if (reason != FighterRefusedReasonEnum.FIGHTER_ACCEPTED)
                {
                    SendChallengeFightJoinRefusedMessage(client, client.Character, reason);
                }
                else
                {
                    //<b>%1</b> agresse <b>%2</b>
                    foreach (var mapClient in target.Map.Clients.Where(mapClient => mapClient != client && mapClient != target.Client))
                    {
                        ContextRoleplayHandler.SendGameRolePlayAggressionMessage(mapClient, client.Character, target);
                    }

                    var fight = Singleton<FightManager>.Instance.CreateAgressionFight(target.Map, 
                        client.Character.AlignmentSide, target.AlignmentSide);

                    fight.ChallengersTeam.AddFighter(client.Character.CreateFighter(fight.ChallengersTeam));
                    fight.DefendersTeam.AddFighter(target.CreateFighter(fight.DefendersTeam));

                    fight.StartPlacement();
                }
            }
        }

        [WorldHandler(GameRolePlayPlayerFightFriendlyAnswerMessage.Id)]
        public static void HandleGameRolePlayPlayerFightFriendlyAnswerMessage(WorldClient client, GameRolePlayPlayerFightFriendlyAnswerMessage message)
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

            var fight = Singleton<FightManager>.Instance.GetFight(message.fightId);

            if (fight == null)
            {
                SendChallengeFightJoinRefusedMessage(client, client.Character, FighterRefusedReasonEnum.TOO_LATE);
                return;
            }

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
            if (fight.ChallengersTeam.Leader.Id == message.fighterId)
                team = fight.ChallengersTeam;
            else if (fight.DefendersTeam.Leader.Id == message.fighterId)
                team = fight.DefendersTeam;
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

            if (!client.Character.Fight.CanKickPlayer)
                return;

            var target = client.Character.Fight.GetOneFighter<CharacterFighter>(message.targetId);

            if (target == null)
                return;

            if (!target.Character.IsFighting())
                return;

            if (client.Character.Fight != target.Character.Fight)
                return;

            client.Character.Fight.KickFighter(client.Character.Fighter, target);
        }

        public static void SendGameFightStartMessage(IPacketReceiver client)
        {
            client.Send(new GameFightStartMessage());
        }

        public static void SendGameFightStartingMessage(IPacketReceiver client, FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightStartingMessage((sbyte) fightTypeEnum));
        }

        public static void SendGameRolePlayShowChallengeMessage(IPacketReceiver client, IFight fight)
        {
            client.Send(new GameRolePlayShowChallengeMessage(fight.GetFightCommonInformations()));
        }

        public static void SendGameRolePlayRemoveChallengeMessage(IPacketReceiver client, IFight fight)
        {
            client.Send(new GameRolePlayRemoveChallengeMessage(fight.Id));
        }

        public static void SendGameFightEndMessage(IPacketReceiver client, IFight fight)
        {
            client.Send(new GameFightEndMessage(fight.GetFightDuration(), fight.AgeBonus, 0, new FightResultListEntry[0]));
        }

        public static void SendGameFightEndMessage(IPacketReceiver client, IFight fight, IEnumerable<FightResultListEntry> results)
        {
            client.Send(new GameFightEndMessage(fight.GetFightDuration(), fight.AgeBonus, 0, results));
        }

        public static void SendGameFightJoinMessage(IPacketReceiver client, bool canBeCancelled, bool canSayReady,
                                                    bool isSpectator, bool isFightStarted, int timeMaxBeforeFightStart,
                                                    FightTypeEnum fightTypeEnum)
        {
            client.Send(new GameFightJoinMessage(canBeCancelled, canSayReady, isSpectator, isFightStarted,
                                                 timeMaxBeforeFightStart, (sbyte) fightTypeEnum));
        }

        public static void SendGameFightSpectateMessage(IPacketReceiver client, IFight fight)
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


        public static void SendGameFightSynchronizeMessage(WorldClient client, IFight fight)
        {
            client.Send(new GameFightSynchronizeMessage(
                    fight.GetAllFighters().Select(entry => entry is SummonedClone ?
                        ((SummonedClone) entry).GetGameFightFighterNamedInformations() :
                        entry.GetGameFightFighterInformations(client))));
        }

        public static void SendGameFightNewRoundMessage(IPacketReceiver client, int roundNumber)
        {
            client.Send(new GameFightNewRoundMessage(roundNumber));
        }

        public static void SendGameFightTurnListMessage(IPacketReceiver client, IFight fight)
        {
            client.Send(new GameFightTurnListMessage(fight.GetAliveFightersIds(), fight.GetDeadFightersIds()));
        }

        public static void SendGameFightTurnStartMessage(IPacketReceiver client, int id, int waitTime)
        {
            client.Send(new GameFightTurnStartMessage(id, waitTime));
        }

        public static void SendGameFightTurnStartSlaveMessage(IPacketReceiver client, int id, int waitTime, int idSummoner)
        {
            client.Send(new GameFightTurnStartSlaveMessage(id, waitTime, idSummoner));
        }

        public static void SendGameFightTurnFinishMessage(IPacketReceiver client)
        {
            client.Send(new GameFightTurnFinishMessage());
        }

        public static void SendGameFightTurnEndMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightTurnEndMessage(fighter.Id));
        }

        public static void SendGameFightUpdateTeamMessage(IPacketReceiver client, IFight fight, FightTeam team)
        {
            client.Send(new GameFightUpdateTeamMessage(
                            (short) fight.Id,
                            team.GetFightTeamInformations()));
        }

        public static void SendGameFightShowFighterMessage(WorldClient client, FightActor fighter)
        {
            var fighterInfos = fighter.GetGameFightFighterInformations(client);

            if (fighter is SummonedClone)
                fighterInfos = (fighter as SummonedClone).GetGameFightFighterNamedInformations();

            client.Send(new GameFightShowFighterMessage(fighterInfos));
        }

        public static void SendGameFightRefreshFighterMessage(WorldClient client, FightActor fighter)
        {
            var fighterInfos = fighter.GetGameFightFighterInformations(client);

            if (fighter is SummonedClone)
                fighterInfos = (fighter as SummonedClone).GetGameFightFighterNamedInformations();

            client.Send(new GameFightRefreshFighterMessage(fighterInfos));
        }

        public static void SendGameFightRemoveTeamMemberMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightRemoveTeamMemberMessage((short) fighter.Fight.Id, (sbyte)fighter.Team.Id, fighter.Id));
        }

        public static void SendGameFightLeaveMessage(IPacketReceiver client, FightActor fighter)
        {
            client.Send(new GameFightLeaveMessage(fighter.Id));
        }

        public static void SendGameFightPlacementPossiblePositionsMessage(IPacketReceiver client, IFight fight, sbyte team)
        {
            client.Send(new GameFightPlacementPossiblePositionsMessage(
                            fight.ChallengersTeam.PlacementCells.Select(entry => entry.Id),
                            fight.DefendersTeam.PlacementCells.Select(entry => entry.Id),
                            team));
        }

        public static void SendGameFightOptionStateUpdateMessage(IPacketReceiver client, FightTeam team, FightOptionsEnum option, bool state)
        {
            client.Send(new GameFightOptionStateUpdateMessage((short) team.Fight.Id, (sbyte)team.Id, (sbyte)option, state));
        }

        public static void SendGameActionFightSpellCastMessage(IPacketReceiver client, ActionsEnum actionId, FightActor caster, FightActor target,
                                                               Cell cell, FightSpellCastCriticalEnum critical, bool silentCast,
                                                               Spell spell)
        {
            client.Send(new GameActionFightSpellCastMessage((short)actionId, caster.Id, target == null ? 0 : target.Id, silentCast ? (short)-1 : cell.Id, (sbyte)(critical),
                                                            silentCast, (short) spell.Id, (sbyte) spell.CurrentLevel));
        }

        public static void SendGameActionFightSpellCastMessage(IPacketReceiver client, ActionsEnum actionId, FightActor caster, FightActor target,
                                                       Cell cell, FightSpellCastCriticalEnum critical, bool silentCast,
                                                       short spellId, sbyte spellLevel)
        {
            client.Send(new GameActionFightSpellCastMessage((short)actionId, caster.Id, target == null ? 0 : target.Id, cell.Id, (sbyte)( critical ),
                                                            silentCast, spellId, spellLevel));
        }

        public static void SendGameActionFightNoSpellCastMessage(IPacketReceiver client, Spell spell)
        {
            client.Send(new GameActionFightNoSpellCastMessage(spell.Id));
        }

        public static void SendGameActionFightModifyEffectsDurationMessage(IPacketReceiver client, FightActor source, FightActor target, short delta)
        {
            client.Send(new GameActionFightModifyEffectsDurationMessage((short)ActionsEnum.ACTION_CHARACTER_BOOST_DISPELLED, source.Id, target.Id, delta));
        }

        public static void SendGameActionFightDispellableEffectMessage(IPacketReceiver client, Buff buff, bool update = false)
        {
            client.Send(new GameActionFightDispellableEffectMessage(update ? (short)ActionsEnum.ACTION_CHARACTER_UPDATE_BOOST : buff.GetActionId(), buff.Caster.Id, buff.GetAbstractFightDispellableEffect()));
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

        public static void SendGameFightTurnReadyRequestMessage(IPacketReceiver client, FightActor current)
        {
            client.Send(new GameFightTurnReadyRequestMessage(current.Id));
        }

        public static void SendSlaveSwitchContextMessage(IPacketReceiver client, SlaveFighter actor)
        {
            client.Send(new SlaveSwitchContextMessage(actor.Summoner.Id, actor.Id, actor.Spells.Select(x => x.GetSpellItem()), actor.GetSlaveCharacteristicsInformations()));
        }
    }
}