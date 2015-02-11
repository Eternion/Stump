using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom
{
    [BrainIdentifier(2864)]
    public class GlourselesteBrain : Brain
    {
        private SpellEffectHandler[] m_gloursongeurHandlers;

        public GlourselesteBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.ActorPushed += OnActorPushed;
            fighter.BeforeDamageInflicted += OnBeforeDamageInflicted;

            fighter.Fight.FightStarted += OnFightStarted;
            fighter.Fight.TurnStarted += OnTurnStarted;
        }

        private void OnFightStarted()
        {
            var gloursongeurSpell = SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.GLOURSONGEUR, 1), Fighter.Cell, false);
            gloursongeurSpell.Initialize();
            m_gloursongeurHandlers = gloursongeurSpell.GetEffectHandlers().ToArray();

            //State Lourd
            m_gloursongeurHandlers[4].Apply();

            Fighter.OpposedTeam.FighterAdded += OnFighterAdded;

            foreach(var fighter in Fighter.OpposedTeam.GetAllFighters())
            {
                fighter.ActorPushed += OnActorPushed;
            }
        }

        private void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (team == Fighter.Team)
                return;

            fighter.ActorPushed += OnActorPushed;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (!(player is CharacterFighter) || player.Team.Id == Fighter.Team.Id)
                return;

            var spell = new Spell((int)SpellIdEnum.GLOURSOMPTUEUX, 1);
            player.CastSpell(spell, Fighter.Cell, true);
        }

        private void OnBeforeDamageInflicted(FightActor fighter, Damage damage)
        {
            if (damage.Source == Fighter)
                return;

            if (!Fighter.Position.Point.IsAdjacentTo(damage.Source.Position.Point))
                return;

            var spell = new Spell((int)SpellIdEnum.HYDROMEL, 1);
            Fighter.CastSpell(spell, Fighter.Cell, true);
        }

        private void OnActorPushed(FightActor fighter, bool takeDamage)
        {
            if (fighter == Fighter)
            {
                m_gloursongeurHandlers[3].Apply(); //State Résuglours
                return;
            }            

            if (takeDamage)
                m_gloursongeurHandlers[1].Apply(); //Kill
        }
    }
}
