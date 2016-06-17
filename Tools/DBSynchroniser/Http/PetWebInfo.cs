
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
        public PetWebFood(string effect, int boostQuantity, FoodType foodType, int foodId, int quantity)
        {
            Effect = effect;
            FoodType = foodType;
            BoostQuantity = boostQuantity;
            FoodId = foodId;
            Quantity = quantity;
        }

        public string Effect
        {
            get;
            set;
        }

        public FoodType FoodType
        {
            get;
            set;
        }

        public int BoostQuantity
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

    public enum FoodType
    {
        Item,
        ItemType,
        Monster
    }
}