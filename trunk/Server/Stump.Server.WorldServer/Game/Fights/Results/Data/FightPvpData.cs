using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Fights.Results.Data
{
    public class FightPvpData : FightResultAdditionalData
    {
        public FightPvpData(Character character)
            : base(character)
        {
        }

        public byte Grade
        {
            get;
            set;
        }

        public ushort MinHonorForGrade
        {
            get;
            set;
        }

        public ushort MaxHonorForGrade
        {
            get;
            set;
        }

        public ushort Honor
        {
            get;
            set;
        }

        public short HonorDelta
        {
            get;
            set;
        }

        public ushort Dishonor
        {
            get;
            set;
        }

        public short DishonorDelta
        {
            get;
            set;
        }

        public override DofusProtocol.Types.FightResultAdditionalData GetFightResultAdditionalData()
        {
            return new FightResultPvpData(Grade, MinHonorForGrade, MaxHonorForGrade, Honor, HonorDelta, Dishonor, DishonorDelta);
        }

        public override void Apply()
        {
            if (HonorDelta > 0)
                Character.AddHonor((ushort)HonorDelta);
            else if (HonorDelta < 0)
                Character.SubHonor((ushort)-HonorDelta);

            if (HonorDelta > 0)
                Character.AddDishonor((ushort)DishonorDelta);
            else if (HonorDelta < 0)
                Character.SubDishonor((ushort)-DishonorDelta);
        }
    }
}