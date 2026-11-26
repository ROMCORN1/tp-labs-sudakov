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

            Console.WriteLine($"Диапазон: до {Limit:N0}");
            Console.WriteLine($"Запрошено потоков: {string.Join(", ", threadCounts)}");
        }
    }
}