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

            if (target == null)
            {
                Log("No target :(");
                return;
            }

            var tree = new PrioritySelector(
                new Decorator(ctx => spell == null, new DecoratorContinue(new FleeAction(Fighter))),
                new PrioritySelector(
                    new Decorator(ctx => Fighter.CanCastSpell(spell, target.Cell),
                                  new Sequence(
                                      new SpellCastAction(Fighter, spell, target.Cell, true),
                                      new DecoratorContinue(ctx => target.LifePoints > Fighter.LifePoints, new FleeAction(Fighter)))),
                    new Sequence(
                        new MoveNearTo(Fighter, target),
                        new Decorator(ctx => Fighter.CanCastSpell(spell, target.Cell),
                                      new Sequence(
                                          new SpellCastAction(Fighter, spell, target.Cell, true),
                                          new Decorator(ctx => target.LifePoints > Fighter.LifePoints, new FleeAction(Fighter)))))));

            foreach (var action in tree.Execute(this))
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