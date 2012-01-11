using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.Worlds.Actors.Fight;
using TreeSharp;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class Brain
    {
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
                Log("Target null !");
            }

            var tree = new PrioritySelector(
                new Decorator(ctx => spell == null, new FleeAction(Fighter)),
                new PrioritySelector(
                    new Decorator(ctx => Fighter.CanCastSpell(spell, target.Cell),
                        new Sequence(
                            new SpellCastAction(Fighter, spell, target.Cell),
                            new DecoratorContinue(ctx => target.LifePoints > Fighter.LifePoints, new FleeAction(Fighter)))),
                     new Sequence(
                         new MoveNearTo(Fighter, target),
                            new Decorator(ctx => Fighter.CanCastSpell(spell, target.Cell),
                                new Sequence(
                                    new SpellCastAction(Fighter, spell, target.Cell),
                                    new Decorator(ctx => target.LifePoints > Fighter.LifePoints, new FleeAction(Fighter)))))));

            foreach (var action in tree.Execute(this))
            {
                // tick
            }

            /*if (spell == null)
            {
                actions.Add(new FleeAction(Fighter));
            }
            else
            {
                

                if (!Fighter.CanCastSpell(spell, target.Cell))
                {
                    actions.Add(new MoveNearTo(Fighter, target));

                    if (Fighter.CanCastSpell(spell, target.Cell))
                    {
                        actions.Add(new SpellCastAction(Fighter, spell, target.Cell));
                        actions.Add(new FleeAction(Fighter));
                    }
                }
                else     
                {
                    actions.Add(new SpellCastAction(Fighter, spell, target.Cell));
                    actions.Add(new FleeAction(Fighter));
                }
            }

            ExecuteActions(actions);*/
        }

        /*public void ExecuteActions(IEnumerable<AIAction> actions)
        {
            foreach (var action in actions)
            {
                action.Execute();
            }
        }*/

        public void Log(string log, params object[] args)
        {
            logger.Debug("Brain " + Fighter + " : " + log, args);
        }
    }
}