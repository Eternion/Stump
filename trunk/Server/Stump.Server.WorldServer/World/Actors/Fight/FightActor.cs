using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.World.Actors.Fight
{
    public abstract class FightActor : ContextActor
    {
        private FightActor m_carriedActor;

        public FightActor CarriedActor
        {
            get { return m_carriedActor; }
            protected set
            {
                m_carriedActor = value;

                m_entityDispositionInformations.Invalidate();
            }
        }

        protected override EntityDispositionInformations BuildEntityDispositionInformations()
        {
            if (CarriedActor != null)
                return new FightEntityDispositionInformations(Position.Cell.Id, (byte) Position.Direction, CarriedActor.Id);

            return base.BuildEntityDispositionInformations();
        }
    }
}