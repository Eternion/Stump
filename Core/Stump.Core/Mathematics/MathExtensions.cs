namespace Stump.Core.Mathematics
{
    public static class MathExtensions
    {
        public static double RoundToNearest(this double amount, double roundTo)
        {
            double excessAmount = amount % roundTo;
            if (excessAmount < (roundTo / 2))
            {
                amount -= excessAmount;
            }
            else
            {
                amount += (roundTo - excessAmount);
            }

            return amount;
        }

        public static double RoundToNearest(this int amount, int roundTo)
        {
            int excessAmount = amount % roundTo;
            if (excessAmount < (roundTo / 2d))
            {
                amount -= excessAmount;
            }
            else
            {
                amount += (roundTo - excessAmount);
            }

            return amount;
        }
    }
}