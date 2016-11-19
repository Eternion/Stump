using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.Sequences
{
    public class FightMoveSequence : FightSequence
    {
        public FightMoveSequence(int id, FightActor author, FightPath path)
            : base(id, SequenceTypeEnum.SEQUENCE_MOVE, author)
        {
            Path = path;
        }

        public FightPath Path
        {
            get;
        }

        protected override TimeSpan DurationWithoutChildren => 
            Path.MPCost > 3 ? TimeSpan.FromSeconds(Path.MPCost * 0.2) : TimeSpan.FromSeconds(Path.MPCost * 0.5);
    }
}