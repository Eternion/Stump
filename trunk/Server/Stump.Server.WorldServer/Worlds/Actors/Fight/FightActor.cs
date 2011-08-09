using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Actors.Fight
{
    public abstract class FightActor : ContextActor
    {
        public FightActor CarriedActor
        {
            get;
            protected set;
        }

        public override EntityDispositionInformations GetEntityDispositionInformations()
        {
            if (CarriedActor != null)
                return new FightEntityDispositionInformations(Position.Cell.Id, (byte) Position.Direction, CarriedActor.Id);

            return base.GetEntityDispositionInformations();
        }
    }
}