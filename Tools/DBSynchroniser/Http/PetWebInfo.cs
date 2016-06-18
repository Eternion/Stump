
using Stump.Server.WorldServer.Database.Items.Pets;

namespace DBSynchroniser.Http
{
    public class PetWebInfo
    {
        public PetWebInfo(int id, PetWebFood[] petWebFood)
        {
            Id = id;
            Foods = petWebFood;
        }

        public int Id
        {
            get;
            set;
        }
        
        public PetWebFood[] Foods
        {
            get;
            set;
        }
    }

    public class PetWebFood
    {
        public PetWebFood(string[] effects, int[] boostQuantities, FoodTypeEnum foodType, int foodId, int quantity)
        {
            Effects = effects;
            FoodType = foodType;
            BoostQuantities = boostQuantities;
            FoodId = foodId;
            Quantity = quantity;
        }

        public string[] Effects
        {
            get;
            set;
        }

        public FoodTypeEnum FoodType
        {
            get;
            set;
        }

        public int[] BoostQuantities
        {
            get;
            set;
        }

        public int FoodId
        {
            get;
            set;
        }

        public int Quantity
        {
            get;
            set;
        }
    }
}