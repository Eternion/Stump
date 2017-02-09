using System.Diagnostics;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using Stump.Server.WorldServer.Game.Spells;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class Brain
    {
        public const int MaxMovesTries = 20;
        public const int MaxCastLimit = 20;

        [Variable(true)]
        public static bool DebugMode = false;

        protected static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Brain(AIFighter fighter)
        {
            Fighter = fighter;
            Environment = new EnvironmentAnalyser(Fighter);
            SpellSelector = new SpellSelector(Fighter, Environment);
        }

        public AIFighter Fighter
        {
            get;
            private set;
        }

        public SpellSelector SpellSelector
        {
            get;
            private set;
        }

        public EnvironmentAnalyser Environment
        {
            get;
            private set;
        }

        public bool IsRange
        {
            get;
            set;
        }

        public virtual void Play()
        {
            Stopwatch sw = null;
            if (BenchmarkManager.Enable)
            {
                sw = Stopwatch.StartNew();
            }

            Environment.ResetMoveZone();

            SpellSelector.AnalysePossibilities();

            if (!Fighter.Fight.AIDebugMode)
            {
                ExecuteSpellCast();
                ExecutePostMove();
            }            
            
            if (sw != null)
            {
                sw.Stop();

                if (sw.ElapsedMilliseconds > 50)
                {
                    BenchmarkManager.Instance.Add(BenchmarkEntry.Create("[AI] " + Fighter, sw.Elapsed, "type", "ai",
                        "spells", SpellSelector.Possibilities.Select(x => x.Spell.ToString()).ToCSV(",")));
                }
            }
        }

        public void ExecutePostMove()
        {
            if (!Fighter.CanMove()) 
                return;

            Action action;
            var hasRangeAttack = SpellSelector.GetRangeAttack(out int minRange, out int maxRange);

            /*if ((Fighter.Stats.MP.Base - Fighter.Stats.MP.Used) > 6)
            {
                action = new FleeAction(Fighter);
            }*/
            if (hasRangeAttack && maxRange > 3 && minRange < maxRange)
            {
                action = new StayInRange(Fighter, minRange, maxRange, Fighter.Spells.Values.Any(x => x.CurrentSpellLevel.CastTestLos));
            }
            else
            {
                action = new MoveNearTo(Fighter, Environment.GetNearestEnemy());
            }

            foreach (var result in action.Execute(this))
            {
                if (result == RunStatus.Failure)
                    break;
            }
        }

        public void ExecuteSpellCast()
        {
            AISpellCastPossibility cast;
            while ((cast = SpellSelector.FindFirstSpellCast()) != null)
            {
                if (cast.MoveBefore != null)
                {
                    var success = Fighter.StartMove(cast.MoveBefore);
                    var lastPos = Fighter.Cell.Id;

                    var tries = 0;
                    var destinationId = cast.MoveBefore.EndCell.Id;
                    // re-attempt to move if we didn't reach the cell i.e as we trigger a trap
                    while (success && Fighter.Cell.Id != destinationId && Fighter.CanMove() && tries <= MaxMovesTries)
                    {
                        var pathfinder = new Pathfinder(Environment.CellInformationProvider);
                        var path = pathfinder.FindPath(Fighter.Position.Cell.Id, destinationId, false, Fighter.MP);

                        if (path == null || path.IsEmpty())
                        {
                            break;
                        }

                        if (path.MPCost > Fighter.MP)
                        {
                            break;
                        }

                        success = Fighter.StartMove(path);

                        // the mob didn't move so we give up
                        if (Fighter.Cell.Id == lastPos)
                        {
                            break;
                        }

                        lastPos = Fighter.Cell.Id;
                        tries++; // avoid infinite loops
                    }

                }

                var targets = Fighter.Fight.GetAllFighters(cast.Target.AffectedCells).ToArray();

                var i = 0;
                while (Fighter.CanCastSpell(cast.Spell, cast.TargetCell.Cell) == SpellCastResult.OK && i < cast.MaxConsecutiveCast)
                {
                    if (!Fighter.CastSpell(cast.Spell, cast.TargetCell.Cell))
                        break;

                    i++;

                    if (Fighter.AP > 0 && targets.All(x => !cast.Target.AffectedCells.Contains(x.Cell)) ||
                        targets.Any(x => x.IsDead())) // target has moved, we re-analyse the situation
                    {
                        SpellSelector.AnalysePossibilities();
                        break;
                    }
                }

                if (i > 0 && i < cast.MaxConsecutiveCast && Fighter.Spells.Values.Any(x => x.CurrentSpellLevel.ApCost <= Fighter.AP))
                    SpellSelector.AnalysePossibilities();
                else
                    break;
            }
        }

        public void Log(string log, params object[] args)
        {
            logger.Debug("Brain " + Fighter + " : " + log, args);

            if (DebugMode)
                Fighter.Say(string.Format(log, args));
        }
    }
}