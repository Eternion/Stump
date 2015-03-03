using System.Diagnostics;
using Stump.Core.Reflection;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class AICommands : SubCommandContainer
    {
        public AICommands()
        {
            Aliases = new[] {"ai"};
            RequiredRole=RoleEnum.GameMaster;
            Description = "Provides commands to manage and debug the ai";
        }
    }

    public class AIDebugCommand : InGameSubCommand
    {
        public AIDebugCommand()
        {
            Aliases = new[] {"debug"};
            RequiredRole=RoleEnum.GameMaster;
            Description = "Enable/disable ai debug mode in current fight";
            ParentCommandType = typeof (AICommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            if (!trigger.Character.IsInFight())
            {
                trigger.ReplyError("You must be in a fight");
                return;
            }

            trigger.Character.Fight.AIDebugMode = !trigger.Character.Fight.AIDebugMode;
            trigger.Reply("AI debug mode " + (trigger.Character.Fight.AIDebugMode ? "enabled" : "disabled"));
        }
    }

    public class AIPassCommand : InGameSubCommand
    {        
        public AIPassCommand()
        {
            Aliases = new[] {"pass"};
            RequiredRole=RoleEnum.GameMaster;
            Description = "Pass current monster turn";
            ParentCommandType = typeof (AICommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            if (!trigger.Character.IsInFight())
            {
                trigger.ReplyError("You must be in a fight");
                return;
            }

            if (!trigger.Character.Fight.AIDebugMode)
            {
                trigger.ReplyError("AI debug mode must be enabled");
                return;
            }

            if (!(trigger.Character.Fight.FighterPlaying is AIFighter))
            {
                trigger.ReplyError("Current fighter is not an AI fighter");
                return;
            }

            trigger.Character.Fight.FighterPlaying.PassTurn();
            trigger.Reply("Turn passed");
        }
    }

    public class AIActionsInfoCommand : InGameSubCommand
    {        
        public AIActionsInfoCommand()
        {
            Aliases = new[] {"actions"};
            RequiredRole=RoleEnum.GameMaster;
            Description = "Get all possible actions of current ai";
            ParentCommandType = typeof (AICommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            if (!trigger.Character.IsInFight())
            {
                trigger.ReplyError("You must be in a fight");
                return;
            }

            if (!trigger.Character.Fight.AIDebugMode)
            {
                trigger.ReplyError("AI debug mode must be enabled");
                return;
            }

            if (!(trigger.Character.Fight.FighterPlaying is AIFighter))
            {
                trigger.ReplyError("Current fighter is not an AI fighter");
                return;
            }

            var aiFighter = trigger.Character.Fight.FighterPlaying as AIFighter;

            foreach (var cast in aiFighter.Brain.SpellSelector.Possibilities)
            {
                trigger.Reply("(#1) Spell {0} ({1}) :: {2}", cast.Spell.Template.Name, cast.Spell.Id, SpellIdentifier.GetSpellCategories(cast.Spell));

                var dumper = new ObjectDumper(8)
                    {
                        MemberPredicate = (member) => !member.Name.Contains("Target")
                    };

                     trigger.Reply("\t{0} Targets", cast.Impacts.Count);
                    foreach (var impact in cast.Impacts)
                    {
                        trigger.Reply(dumper.DumpElement(impact));
                        if (impact.Target != null)
                            trigger.Reply("\t\tTarget = " + impact.Target is NamedFighter ? ( (NamedFighter)impact.Target ).Name : impact.Target.Id.ToString());
                    }
            }
        }
    }
    public class AIActionExecuteCommand : InGameSubCommand
    {        
        public AIActionExecuteCommand()
        {
            Aliases = new[] {"exec"};
            RequiredRole=RoleEnum.GameMaster;
            Description = "Force an ai to execute an action";
            ParentCommandType = typeof (AICommands);
            AddParameter("action", "a", "Index of the action to execute", 0);
        }

        public override void Execute(GameTrigger trigger)
        {
            if (!trigger.Character.IsInFight())
            {
                trigger.ReplyError("You must be in a fight");
                return;
            }

            if (!trigger.Character.Fight.AIDebugMode)
            {
                trigger.ReplyError("AI debug mode must be enabled");
                return;
            }

            if (!(trigger.Character.Fight.FighterPlaying is AIFighter))
            {
                trigger.ReplyError("Current fighter is not an AI fighter");
                return;
            }

            var aiFighter = trigger.Character.Fight.FighterPlaying as AIFighter;

            int index = trigger.Get<int>("action");

            if (index >= aiFighter.Brain.SpellSelector.Possibilities.Count)
            {
                trigger.ReplyError("Specify an index between 0 and " + (aiFighter.Brain.SpellSelector.Possibilities.Count - 1));
                return;
            }

            var cast = aiFighter.Brain.SpellSelector.Possibilities[index];

            //aiFighter.Brain.ExecuteSpellCast(cast);
        }
    }

    public class AIShowMovesCommand : InGameSubCommand
    {
        public AIShowMovesCommand()
          {
            Aliases = new[] {"showmoves"};
            RequiredRole=RoleEnum.GameMaster;
            Description = "Show different possibles moves";
            ParentCommandType = typeof (AICommands);
            AddParameter("los", "los", "Show LoS", true, true);
          }

        public override void Execute(GameTrigger trigger)
        {
            if (!trigger.Character.IsInFight())
            {
                trigger.ReplyError("You must be in a fight");
                return;
            }

            if (!trigger.Character.Fight.AIDebugMode)
            {
                trigger.ReplyError("AI debug mode must be enabled");
                return;
            }

            if (!(trigger.Character.Fight.FighterPlaying is AIFighter))
            {
                trigger.ReplyError("Current fighter is not an AI fighter");
                return;
            }

            var aiFighter = trigger.Character.Fight.FighterPlaying as AIFighter;

            trigger.Character.ClearHighlight();

            var enemy =aiFighter.Brain.Environment.GetNearestEnemy();

            if (trigger.IsArgumentDefined("los"))
                trigger.Reply(trigger.Color("Cast with LoS", trigger.Character.HighlightCells(aiFighter.Brain.Environment.GetCellsWithLoS(enemy.Cell))));

            foreach (var spell in aiFighter.Spells.Values)
            {
                var cell = aiFighter.Brain.Environment.GetCellToCastSpell(enemy.Cell, spell);
                if (cell != null)
                    trigger.Reply(trigger.Color("Cast " + spell, trigger.Character.HighlightCell(cell)));
            }

            var fleeCell = aiFighter.Brain.Environment.GetCellToFlee();
            trigger.Reply(trigger.Color("Flee cell", trigger.Character.HighlightCell(fleeCell)));

        }
    }


}