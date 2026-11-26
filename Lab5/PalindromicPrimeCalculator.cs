using System.Collections.Concurrent;

namespace Lab5
{
    public static class PalindromicPrimeCalculator
    {
        public static (long Count, long[] Largest) FindSequential(long limit)
        {
            var palindromes = GeneratePalindromes(limit);
            var primes = new List<long>();

            foreach (var p in palindromes)
                if (IsPrime(p)) primes.Add(p);

            primes.Sort();
            var largest = primes.TakeLast(10).ToArray();
            return (primes.Count, largest);
        }

        public static (long Count, long[] Largest) FindParallel(long limit, int threadCount)
        {
            var palindromes = GeneratePalindromes(limit);
            var primes = new ConcurrentBag<long>();

            var options = new ParallelOptions { MaxDegreeOfParallelism = threadCount };
            Parallel.ForEach(palindromes, options, p =>
            {
                if (IsPrime(p)) primes.Add(p);
            });

            var sorted = primes.OrderBy(x => x).ToArray();
            var largest = sorted.TakeLast(10).ToArray();
            return (sorted.Length, largest);
        }

        public static List<long> GeneratePalindromes(long limit)
        {
            var result = new List<long>();
            for (int len = 1; len <= 8; len++)
            {
                int halfLen = (len + 1) / 2;
                long start = (long)Math.Pow(10, halfLen - 1);
                long end   = (long)Math.Pow(10, halfLen) - 1;

                for (long i = start; i <= end; i++)
                {
                    string half = i.ToString();
                    string rev  = new string(half.Reverse().ToArray());
                    string palStr = (len % 2 == 0)
                        ? half + rev
                        : half + rev.Substring(1);

                    long pal = long.Parse(palStr);
                    if (pal <= limit) result.Add(pal);
                }
            }
            return result;
        }

        public static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;

            for (long i = 5; i * i <= n; i += 6)
                if (n % i == 0 || n % (i + 2) == 0) return false;

            return true;
        }
    }
}