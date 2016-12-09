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

        public static string ToBase(this short num, int nbase)
        {
            return ToBase((int)num, nbase);
        }

        public static string ToBase(this int num, int nbase)
        {
            string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // check if we can convert to another base
            if (nbase < 2 || nbase > chars.Length)
                return "";

            int r;
            string newNumber = "";

            // in r we have the offset of the char that was converted to the new base
            while (num >= nbase)
            {
                r = num % nbase;
                newNumber = chars[r] + newNumber;
                num = num / nbase;
            }
            // the last number to convert
            newNumber = chars[num] + newNumber;

            return newNumber;
        }
    }
}