using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Challenges;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class FightCommands : SubCommandContainer
    {
        public FightCommands()
        {
            Aliases = new[] {"fight"};
            Description = "Provides commands to manage fights";
            RequiredRole=RoleEnum.GameMaster;
        }
    }

    public class KickFightCommand : TargetSubCommand
    {
        public KickFightCommand()
        {
            Aliases = new[] {"kick"};
            Description = "Kick the target";
            ParentCommandType = typeof (FightCommands);
            RequiredRole = RoleEnum.GameMaster;
            AddTargetParameter();
        }

        public override void Execute(TriggerBase trigger)
        {
            foreach (var target in GetTargets(trigger))
            {
                if (!target.IsInFight())
                    trigger.ReplyError("{0} is not fighting", target);

                else
                {
                    var fight = target.Fight;
                    if (target.IsFighting())
                        target.Fighter.LeaveFight();
                    if (target.IsSpectator())
                        target.Spectator.Leave();

                    trigger.ReplyBold("{0} get kicked from fight {1}", target, fight.Id);
                }
            }
        }
    }

    public class ListFightCommand : SubCommand
    {
        public ListFightCommand()
        {
            Aliases = new[] {"list"};
            Description = "List fights on the map";
            ParentCommandType = typeof (FightCommands);
            RequiredRole = RoleEnum.GameMaster;
            AddParameter("map", "m", "List fights of that map", isOptional: true,
                              converter: ParametersConverter.MapConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            Map map;
            if (!trigger.IsArgumentDefined("map"))
            {
                if (!(trigger is GameTrigger))
                {
                    trigger.ReplyError("Map not defined");
                    return;
                }

                map = ((GameTrigger) trigger).Character.Map;
            }
            else
                map = trigger.Get<Map>("map");

            foreach (var fight in map.Fights)
            {
                trigger.ReplyBold(" - {0} (red:{1}, blue{2}){3}", fight.Id, fight.DefendersTeam.Fighters.Count,
                    fight.ChallengersTeam.Fighters.Count, fight.State == FightState.Placement ? " Placement phase" : string.Empty);
            }
        }
    }

    public class JoinFightCommand : TargetSubCommand
    {
        public JoinFightCommand()
        {
            Aliases = new[] { "join" };
            Description = "Join a fight";
            ParentCommandType = typeof(FightCommands);
            RequiredRole = RoleEnum.GameMaster;
            AddParameter("fight", "f", "The fight to join",
                              converter: ParametersConverter.FightConverter);
            AddParameter("team", "team", "Team to join (red or blue)", defaultValue: "red");
            AddTargetParameter(true, "The character that will join the fight");
        }

        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var fight = trigger.Get<IFight>("fight");
            var teamString = trigger.Get<string>("team");

            if (!teamString.Equals("blue", StringComparison.InvariantCultureIgnoreCase) &&
                !teamString.Equals("red", StringComparison.InvariantCultureIgnoreCase))
            {
                trigger.ReplyError("Specify a team (red or blue)");
                return;
            }

            var joinRed = teamString.Equals("red", StringComparison.InvariantCultureIgnoreCase);

            if (target.IsFighting())
            {
                if (target.Fight == fight)
                {
                    trigger.ReplyError("{0} is already in the given fight", target);
                    return;
                }

                target.Fighter.LeaveFight();
            }

            var team = joinRed ? fight.ChallengersTeam : fight.DefendersTeam;
            var fighter = target.CreateFighter(team);

            if (fighter == null)
            {
                trigger.ReplyError("Cannot join fight for an unknown reason");
                return;
            }

            if (!fight.FindRandomFreeCell(fighter, out var cell))
                foreach (var ally in team.Fighters)
                {
                    foreach (var point in ally.Position.Point.GetAdjacentCells(x => fight.IsCellFree(fight.Map.Cells[x])))
                    {
                        cell = fight.Map.GetCell(point.CellId);
                        break;
                    }

                    if (cell != null)
                        break;
                }

            if (cell == null)
            {
                target.RejoinMap();
                trigger.ReplyError("{0} cannot join fight {1}, no free cell were found !", target, fight.Id);
                return;
            }

            fighter.Cell = cell;

            team.AddFighter(fighter);
            trigger.ReplyBold("{0} joined fight {1}", target, fight.Id);
        }
    }

    public class EndFightCommand : SubCommand
    {
        public EndFightCommand()
        {
            Aliases = new[] { "end", "stop" };
            Description = "Ends a fight";
            ParentCommandType = typeof(FightCommands);
            RequiredRole = RoleEnum.GameMaster;
            AddParameter("fight", "f", "The fight to end", isOptional: true,
                              converter: ParametersConverter.FightConverter);

        }

        public override void Execute(TriggerBase trigger)
        {
            IFight fight;
            if (trigger.IsArgumentDefined("fight"))
                fight = trigger.Get<IFight>("fight");
            else if ((trigger is GameTrigger) && ((GameTrigger) trigger).Character.IsInFight())
                fight = ((GameTrigger) trigger).Character.Fight;
            else
            {
                trigger.ReplyError("Define a fight");
                return;
            }

            fight.EndFight();

            trigger.ReplyBold("Fight {0} ended", fight.Id);
        }
    }

    public class PassTurnFightCommand : SubCommand
    {
        public PassTurnFightCommand()
        {
            Aliases = new[] { "pass" };
            Description = "Pass the current turn in the given fight";
            ParentCommandType = typeof(FightCommands);
            RequiredRole = RoleEnum.GameMaster;
            AddParameter("fight", "f", "The fight to end", isOptional: true,
                              converter: ParametersConverter.FightConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            IFight fight;
            if (trigger.IsArgumentDefined("fight"))
                fight = trigger.Get<IFight>("fight");
            else if (( trigger is GameTrigger ) && ( trigger as GameTrigger ).Character.IsInFight())
                fight = ( trigger as GameTrigger ).Character.Fight;
            else
            {
                trigger.ReplyError("Define a fight");
                return;
            }

            fight.FighterPlaying.PassTurn();
        }
    }

    public class FreezeFightCommand : SubCommand
    {
        public FreezeFightCommand()
        {
            Aliases = new[] { "freeze" };
            Description = "Freeze/unfreeze current fight";
            ParentCommandType = typeof(FightCommands);
            RequiredRole = RoleEnum.GameMaster;
            AddParameter("fight", "f", "The fight to end", isOptional: true,
                              converter: ParametersConverter.FightConverter);
        }

        public override void Execute(TriggerBase trigger)
        {
            IFight fight;
            if (trigger.IsArgumentDefined("fight"))
                fight = trigger.Get<IFight>("fight");
            else if (( trigger is GameTrigger ) && ( (GameTrigger) trigger ).Character.IsInFight())
                fight = ( (GameTrigger) trigger ).Character.Fight;
            else
            {
                trigger.ReplyError("Define a fight");
                return;
            }

            fight.Freezed = !fight.Freezed;
            trigger.Reply("Fight " + (fight.Freezed ? "freezed" : "unfreezed"));
        }
    }

    public class ChallengeFightCommand : SubCommand
    {
        public ChallengeFightCommand()
        {
            Aliases = new[] { "challenge" };
            Description = "Set challenge for current fight";
            ParentCommandType = typeof(FightCommands);
            RequiredRole = RoleEnum.GameMaster;
            AddParameter<int>("challenge", "c", "Challenge Id");
        }

        public override void Execute(TriggerBase trigger)
        {
            if ((trigger is GameTrigger) && !((GameTrigger)trigger).Character.IsInFight())
            {
                trigger.ReplyError("Must be in fight");
                return;
            }
            
            var challengeId = trigger.Get<int>("challenge");
            var fight = ((GameTrigger)trigger).Character.Fight;

            var challenge = ChallengeManager.Instance.GetChallenge(challengeId, fight);
            challenge.Initialize();

            fight.AddChallenge(challenge);

            trigger.Reply("Force challengeId {0} for current Fight", challenge.Id);
        }
    }
}