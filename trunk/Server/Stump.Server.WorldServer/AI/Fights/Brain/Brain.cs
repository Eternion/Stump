using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.AI.Fights.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Maps.Pathfinding;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class Brain
    {
        public const int MaxMovesTries = 20;
        public const int MaxCastLimit = 20;


        [Variable(true)]
        public static bool DebugMode = true;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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

        public void Play()
        {
            SpellSelector.AnalysePossibilities();
            foreach (var cast in SpellSelector.EnumerateSpellsCast())
            {
                if (cast.MoveBefore != null)
                {
                    Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                    bool success = Fighter.StartMove(cast.MoveBefore);
                    var lastPos = Fighter.Cell.Id;

                    int tries = 0;
                    var destinationId = cast.MoveBefore.EndCell.Id;
                    // re-attempt to move if we didn't reach the cell i.e as we trigger a trap
                    while (success && Fighter.Cell.Id != destinationId && Fighter.CanMove() && tries <= MaxMovesTries)
                    {
                        var pathfinder = new Pathfinder(Environment.CellInformationProvider);
                        var path = pathfinder.FindPath(Fighter.Position.Cell.Id, destinationId, false, Fighter.MP);

                        if (path == null || path.IsEmpty())
                        {
                            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                            continue;
                        }

                        if (path.MPCost > Fighter.MP)
                        {
                            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                            continue;
                        }

                        success = Fighter.StartMove(path);

                        // the mob didn't move so we give up
                        if (Fighter.Cell.Id == lastPos)
                        {
                            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                            continue;
                        }

                        lastPos = Fighter.Cell.Id;
                        tries++; // avoid infinite loops
                    }

                    Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_MOVE);
                }

                int i = 0;
                while (Fighter.CanCastSpell(cast.Spell, cast.TargetCell) && i <= MaxCastLimit)
                {
                    if (!Fighter.CastSpell(cast.Spell, cast.TargetCell))
                        break;

                    i++;
                }
            }

            if (Fighter.CanMove())
            {
                foreach (var action in new MoveNearTo(Fighter, Environment.GetNearestEnnemy()).Execute(this))
                {

                }
            }

            /*var spell = SpellSelector.GetBestSpell();
            var target = Environment.GetNearestEnnemy();

            var selector = new PrioritySelector();
            selector.AddChild(new Decorator(ctx => target == null, new DecoratorContinue(new RandomMove(Fighter))));
            selector.AddChild(new Decorator(ctx => spell == null, new DecoratorContinue(new FleeAction(Fighter))));

            if (target != null && spell != null)
            {
                selector.AddChild(new PrioritySelector(
                                      new Decorator(ctx => Fighter.CanCastSpell(spell, target.Cell),
                                                    new Sequence(
                                                        new SpellCastAction(Fighter, spell, target.Cell, true),
                                                        new PrioritySelector(
                                                            new Decorator(
                                                                ctx => target.LifePoints > Fighter.LifePoints,
                                                                new FleeAction(Fighter)),
                                                            new Decorator(new MoveNearTo(Fighter, target))))),
                                      new Sequence(
                                          new MoveNearTo(Fighter, target),
                                          new Decorator(ctx => Fighter.CanCastSpell(spell, target.Cell),
                                                        new Sequence(
                                                            new SpellCastAction(Fighter, spell, target.Cell, true),
                                                            new Decorator(
                                                                ctx => target.LifePoints > Fighter.LifePoints,
                                                                new FleeAction(Fighter)))))));
            }

            foreach (var action in selector.Execute(this))
            {
                // tick the tree
            }*/
        }

        public void Log(string log, params object[] args)
        {
            logger.Debug("Brain " + Fighter + " : " + log, args);

            if (DebugMode)
                Fighter.Say(string.Format(log, args));
        }
    }
}