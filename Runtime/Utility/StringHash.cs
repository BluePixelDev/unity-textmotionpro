namespace BP.TextMotionPro
{
    public static class StringHash
    {
        private const uint FNVOffsetBasis = 2166136261;
        private const uint FNVPrime = 16777619;

        public static int FNV1aHash(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 0;

            unchecked
            {
                uint hash = FNVOffsetBasis;
                for (int i = 0; i < input.Length; i++)
                {
                    hash ^= input[i];
                    hash *= FNVPrime;
                }

                return (int)hash;
            }
        }
    }
}
