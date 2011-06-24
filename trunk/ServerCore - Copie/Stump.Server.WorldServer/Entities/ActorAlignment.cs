using Stump.Database.WorldServer.Alignment;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.World.Actors
{
    public class ActorAlignment
    {
        public ActorAlignment(IAligned actor, AlignmentRecord record)
        {
            Actor = actor;
            Record = record;
        }

        public IAligned Actor
        {
            get;
            private set;
        }

        public AlignmentRecord Record
        {
            get;
            private set;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get { return Record.AlignmentSide; }
            set { Record.AlignmentSide = value; }
        }

        public uint AlignmentValue
        {
            get { return Record.AlignmentValue; }
            set { Record.AlignmentValue = value; }
        }

        public uint AlignmentGrade
        {
            get;
            private set;
        }

        public uint Honor
        {
            get { return Record.Honor; }
            set
            {
                Record.Honor = value;
                AlignmentGrade = ThresholdManager.Thresholds["GradeExp"].GetLevel(Honor);
            }
        }

        public uint Dishonor
        {
            get { return Record.Dishonor; }
            set { Record.Dishonor = value; }
        }


        public uint CharacterPower
        {
            get { return (uint) (Actor.Id + Actor.Level); }
        }

        public ActorAlignmentInformations ToActorAlignmentInformations()
        {
            return new ActorAlignmentInformations((int) AlignmentSide, AlignmentValue, AlignmentGrade, Dishonor,
                                                  CharacterPower);
        }
    }
}