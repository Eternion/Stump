using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Alignment;
using Stump.DofusProtocol.Classes;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.World.Actors
{
    public class ActorAlignment
    {

        public ActorAlignment(IAligned actor, int alignmentSide, uint alignmentValue, uint honor, uint dishonor)
        {
            Actor = actor;

            AlignmentSide = alignmentSide;
            AlignmentValue =alignmentValue;
            Honor = honor;
            Dishonor = dishonor;
        }

        public ActorAlignment(IAligned actor, AlignmentRecord record)
        {
            Actor = actor;

            AlignmentSide = record.AlignmentSide;
            AlignmentValue = record.AlignmentValue;
            Honor = record.Honor;
            Dishonor = record.Dishonor;
        }

        public readonly IAligned Actor;

        public readonly AlignmentRecord Record;

        public int AlignmentSide
        {
            get;
            set;
        }

        public uint AlignmentValue
        {
            get;
            set;
        }

        public uint AlignmentGrade
        {
            get { return ThresholdManager.Thresholds["GradeExp"].GetLevel(Honor); }
            set { Honor = (uint)ThresholdManager.Thresholds["GradeExp"].GetLowerBound(Honor); }
        }

        public uint Honor
        {
            get;
            set;
        }

        public uint Dishonor
        {
            get;
            set;
        }

        public long CharacterPower
        {
            get { return Actor.Id + Actor.Level; }
        }

        public void Save()
        {
            if (Record != null)
            {
                Record.AlignmentSide = AlignmentSide;
                Record.AlignmentValue = AlignmentValue;
                Record.Honor = Honor;
                Record.Dishonor = Dishonor;
                Record.SaveAndFlush();
            }
        }

        public ActorAlignmentInformations ToActorAlignmentInformations()
        {
            return new ActorAlignmentInformations(AlignmentSide, AlignmentValue, AlignmentGrade, Dishonor, CharacterPower);
        }

    }
}
