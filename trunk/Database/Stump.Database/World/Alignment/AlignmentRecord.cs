
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.WorldServer.Alignment
{
    [ActiveRecord("characters_alignment")]
    public sealed class AlignmentRecord : WorldBaseRecord<AlignmentRecord>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "CharacterId")]
        public uint CharacterId
        {
            get;
            set;
        }

        [OneToOne]
        public CharacterRecord Character
        {
            get;
            set;
        }

        [Property("AlignmentSide", NotNull = true, Default = "0")]
        public AlignmentSideEnum AlignmentSide
        {
            get;
            set;
        }

        [Property("AlignmentValue", NotNull = true, Default = "0")]
        public uint AlignmentValue
        {
            get;
            set;
        }

        [Property("Honour", NotNull = true, Default = "0")]
        public uint Honor
        {
            get;
            set;
        }

        [Property("Dishonor", NotNull = true, Default = "0")]
        public uint Dishonor
        {
            get;
            set;
        }
    }
}