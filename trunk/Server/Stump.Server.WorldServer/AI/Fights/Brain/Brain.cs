using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.Game.Actors.Fight;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class Brain
    {
        [Variable(true)]
        public static bool DebugMode = true;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public Brain(AIFighter fighter)
        {
            Fighter = fighter;
            SpellSelector = new SpellSelector(Fighter);
            Environment = new EnvironmentAnalyser(Fighter);
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
            var spell = SpellSelector.GetBestSpell();
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