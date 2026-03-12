namespace SpaceInvaderTemplate.Utils
{
    public static class PrimeSumHealthDisplayUtil
    {
        public static readonly int[] Primes = {
            1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97
        };

        public static int GetHealthDisplay(int h)
        {
            if (h > Primes.Length)
            {
                return int.MaxValue;
            }
            int sum = 0;
            for (int i = 0; i < h; i++)
            {
                sum += Primes[i];
            }

            return sum;
        }
    }
}