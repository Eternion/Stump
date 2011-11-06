using System.Collections.Generic;
using Stump.Server.WorldServer.AI.Fights.Actions;
using Stump.Server.WorldServer.Worlds.Actors.Fight;

namespace Stump.Server.WorldServer.AI.Fights.Brain
{
    public class Brain
    {
        public Brain(AIFighter fighter)
        {
            Fighter = fighter;
            SpellSelector = new SpellSelector(Fighter);
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


        public void Play()
        {
            var actions = new List<AIAction>();
            var spell = SpellSelector.GetBestSpell();

            if (spell == null)
            {
                actions.Add(new MoveFarFrom(Fighter, Fighter.GetNearestEnnemy()));
            }
            else
            {
                var target = Fighter.GetNearestEnnemy();

                if (!Fighter.CanCastSpell(spell, target.Cell))
                {
                    actions.Add(new MoveNearTo(Fighter, target));

                    if (Fighter.CanCastSpell(spell, target.Cell))
                    {
                        actions.Add(new SpellCastAction(Fighter, spell, target.Cell));
                        actions.Add(new MoveFarFrom(Fighter, target));
                    }
                }
                else     
                {
                    actions.Add(new SpellCastAction(Fighter, spell, target.Cell));
                    actions.Add(new MoveFarFrom(Fighter, target));
                }
            }

            ExecuteActions(actions);
        }

        public void ExecuteActions(IEnumerable<AIAction> actions)
        {
            foreach (var action in actions)
            {
                action.Execute();
            }
        }
    }
}