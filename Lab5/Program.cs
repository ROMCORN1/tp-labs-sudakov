using System.Diagnostics;

namespace Lab5
{
    internal class Program
    {
        static void Main()
        {
            const long Limit = 100_000_000;

            Console.Write("Введите количества потоков через пробел: ");
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ошибка: ничего не введено.");
                return;
            }

            var threadCounts = new List<int>();
            foreach (string part in input.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (!int.TryParse(part, out int count) || count <= 0)
                {
                    Console.WriteLine($"Ошибка: '{part}' — некорректное количество потоков.");
                    return;
                }
                if (!threadCounts.Contains(count)) threadCounts.Add(count);
            }

            Console.WriteLine("\nПоследовательное вычисление:");
            var sw = Stopwatch.StartNew();
            var seq = PalindromicPrimeCalculator.FindSequential(Limit);
            sw.Stop();
            long seqTime = sw.ElapsedMilliseconds;

            Console.WriteLine($"Количество простых палиндромов: {seq.Count}");
            Console.WriteLine($"10 наибольших: {string.Join(", ", seq.Largest)}");
            Console.WriteLine($"Время: {seqTime} мс");

            Console.WriteLine("\nМногопоточные вычисления:");
            Console.WriteLine($"{"Потоков",-10}{"Время, мс",-15}{"Ускорение",-15}{"Совпадение",-15}");
            Console.WriteLine(new string('-', 55));

            foreach (int tc in threadCounts)
            {
                sw.Restart();
                var par = PalindromicPrimeCalculator.FindParallel(Limit, tc);
                sw.Stop();

                long parTime = sw.ElapsedMilliseconds;
                double speedup = parTime == 0 ? 0 : (double)seqTime / parTime;
                bool match = par.Count == seq.Count && par.Largest.SequenceEqual(seq.Largest);

                Console.WriteLine($"{tc,-10}{parTime,-15}{speedup,-15:F2}{(match ? "Да" : "Нет"),-15}");
            }
        }
    }
}